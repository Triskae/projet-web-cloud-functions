using System;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using ProjetWeb.Models;

namespace ProjetWeb.Utils
{
    public static class UserUtils
    {
        public static User GetUserFromEmail(DocumentClient documentClient, string email)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri("ProjetWeb", "Users");
            var queryOptions = new FeedOptions {EnableCrossPartitionQuery = true};
            var query = documentClient.CreateDocumentQuery<Models.User>(collectionUri, queryOptions)
                .Where(u => u.Email == email)
                .AsDocumentQuery();
            
            var result = query.ExecuteNextAsync<Models.User>().Result.ToList();
            return result.FirstOrDefault();
        }
    }
}