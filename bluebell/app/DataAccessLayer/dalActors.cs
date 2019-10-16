using Helium.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Linq;

namespace Helium.DataAccessLayer
{
    /// <summary>
    /// Data Access Layer for CosmosDB
    /// </summary>
    public partial class DAL
    {
        // select template for Actors
        const string actorSelect = "select m.id, m.partitionKey, m.actorId, m.type, m.name, m.birthYear, m.deathYear, m.profession, m.textSearch, m.movies from m where m.type = 'Actor' ";

        /// <summary>
        /// Retrieve a single actor from CosmosDB by actorId
        /// 
        /// Uses the CosmosDB single document read API which is 1 RU if less than 1K doc size
        /// 
        /// Throws an exception if not found
        /// </summary>
        /// <param name="actorId">Actor ID</param>
        /// <returns>Actor object</returns>
        public async System.Threading.Tasks.Task<Actor> GetActorAsync(string actorId)
        {
            // get the partition key for the actor ID
            // note: if the key cannot be determined from the ID, ReadDocumentAsync cannot be used.
            // GetPartitionKey will throw an ArgumentException if the actorId isn't valid
            RequestOptions requestOptions = new RequestOptions { PartitionKey = new PartitionKey(GetPartitionKey(actorId)) };

            // get an actor by ID
            return await client.ReadDocumentAsync<Actor>(collectionLink.ToString() + "/docs/" + actorId, requestOptions);
        }

        /// <summary>
        /// Get all Actors from CosmosDB
        /// </summary>
        /// <returns>List of Actors</returns>
        public IQueryable<Actor> GetActors()
        {
            // get all actors
            return GetActorsByQuery(string.Empty);
        }

        /// <summary>
        /// Get Actors by search string
        /// 
        /// The search is a "contains" search on actor name
        /// If q is empty, all actors are returned
        /// </summary>
        /// <param name="q">search term</param>
        /// <returns>a list of Actors or an empty list</returns>
        public IQueryable<Actor> GetActorsByQuery(string q)
        {
            if (q == null)
            {
                q = string.Empty;
            }

            // convert to lower and escape embedded '
            q = q.Trim().ToLower().Replace("'", "''");

            string sql = actorSelect + " order by m.actorId";

            if (!string.IsNullOrEmpty(q))
            {
                // get actors by a "like" search on name
                sql = string.Format("{0} and contains(m.textSearch, '{1}') order by m.actorId", actorSelect, q);
            }

            return QueryActorWorker(sql);
        }

        /// <summary>
        /// Actor Worker Query
        /// </summary>
        /// <param name="sql">select statement to execute</param>
        /// <returns>List of Actors</returns>
        public IQueryable<Actor> QueryActorWorker(string sql)
        {
            // run query
            return client.CreateDocumentQuery<Actor>(collectionLink, sql, feedOptions);
        }
    }
}