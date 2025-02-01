using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using Oversite.Helpers;

namespace Oversite.Helpers
{
    public class MongoDbConnectionHandler
    {
        #region "Constructor"
        AppConfigManager appConfiguration = new AppConfigManager();
        public MongoDbConnectionHandler()
            {
                mongoServerSettings = new MongoServerSettings();
            //mongoServerSettings.Server = new MongoServerAddress("10.150.10.212", 27017);// live 31/3/2020
           // mongoServerSettings.Server = new MongoServerAddress("10.192.5.55", 27017);//test
            // mongoServerSettings.Server = new MongoServerAddress(appConfiguration.MongoDBIP,Convert.ToInt32(appConfiguration.MongoDBPort));
            //mongoServerSettings.Server = new MongoServerAddress("10.194.2.77", 27017);//26/10/2020
            //mongoServerSettings.Server = new MongoServerAddress("10.124.0.8", 27017);
            //mongoServerSettings.Server = new MongoServerAddress("10.192.5.55", 27017); //mafiltest commented on 5/march/2020
            //mongoServerSettings.Server = new MongoServerAddress("10.124.0.8", 27017);

            //mongoServer = new MongoServer(mongoServerSettings);
            //mongoDatabase = mongoServer.GetDatabase(appConfiguration.MongoDBName);


            MongoClient client = new MongoClient(appConfiguration.MongoDBSERVER);
            mongoServer = client.GetServer();
            mongoDatabase = mongoServer.GetDatabase(appConfiguration.MongoDBName);
        }
        #endregion

        #region "Properties"
            MongoServerSettings mongoServerSettings;
            MongoServer mongoServer;
            MongoDatabase mongoDatabase;
        #endregion

        #region "Methods"
        public void Insert<T>(string collectionName ,T value )
        {
            try
            {
                mongoServer.Connect();
                MongoCollection<T>  mongoCollection = mongoDatabase.GetCollection<T>(collectionName);
                BsonDocument doc = value.ToBsonDocument();
                mongoCollection.Insert(doc);
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mongoServer.Disconnect();
            }
        }

        public void Update<T>(string collectionName, T value,string feildName,int id)
        {
            try
            {
                mongoServer.Connect();
                MongoCollection<T> mongoCollection = mongoDatabase.GetCollection<T>(collectionName);
                IMongoQuery Marker = Query.EQ(feildName, id);
                BsonDocument doc = value.ToBsonDocument();
                mongoCollection.Insert(doc);
            }

            catch
            {
                throw;
            }
            finally
            {
                mongoServer.Disconnect();
            }
        }
        public T select<T>(string collectionName, string feildName, string id)
        {
            try
            {
                mongoServer.Connect();
                T obj;
                IMongoQuery Marker = Query.EQ(feildName, id);
                obj = mongoDatabase.GetCollection<T>(collectionName).Find(Marker).FirstOrDefault();
                return obj;
            }

            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                mongoServer.Disconnect();
            }


        }
        public T Select<T>(string collectionName, string feildName, int id)
        {
            try
            {
                mongoServer.Connect();
                T obj;
                IMongoQuery Marker = Query.EQ(feildName, id);
                obj = mongoDatabase.GetCollection<T>(collectionName).Find(Marker).FirstOrDefault();
                return obj;
            }

            catch
            {
                throw;
            }
            finally
            {
                mongoServer.Disconnect();
            }


        }
        //public List<T> Selectnew<T>(string collectionName, string feildName1, int id)
        //{
        //    try
        //    {
        //        mongoServer.Connect();

        //        List<T> obj;
        //        IMongoQuery Marker = Query.EQ(feildName1, id);
        //        obj = mongoDatabase.GetCollection<T>(collectionName).FindAll().ToList();
        //        return obj;
        //    }

        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        mongoServer.Disconnect();
        //    }


        //}
        public List<T> SelectAll<T>(string collectionName, string feildName, int id)
        {
            try
            {
                mongoServer.Connect();
                List<T> obj = new List<T>();
                IMongoQuery Marker = Query.EQ(feildName, id);
                obj = mongoDatabase.GetCollection<T>(collectionName).Find(Marker).ToList();
                return obj;
            }
            catch
            {
                throw;
            }
            finally
            {
                mongoServer.Disconnect();
            }
        }

        #endregion




    }

}
