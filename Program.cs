using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using UF3_test.connections;
using UF3_test.model;

namespace UF3_test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //EA1
            //GetAllDBs();         
            //GetCollections();
            //SelectAllStudents();
            //InsertOneStudent();
            //SelectOneStudent();
            //SelectStudentFields();            


            //EA2
            //LoadPeopleCollection();
            //SelectPeopleByFriend();
            //SelectPeopleByAge();
            //UpdateOnePerson();
            //UpdateManyPeople();
            //DeleteOnePerson();
            //DeleteManyPeople();
            //UpdatePeopleArrayPopLast();
            //UpdatePeopleNewField();


            // DropCollection("itb", "people");
            // DropCollection("itb", "products");
            // DropCollection("itb", "books");

            //carreguem documents sense _id
            // loadProductsDocsCollection();
            //Quan Obtenim els objectes falla
            //SelectAllProducts(); 

            //carreguem documents amb _id nostre
            //loadProducts2DocsCollection();
            //Obtenim els objectes amb _id nostre
            //SelectAllProducts2();

            //Si intentem carregar documents sense el _id falla 
            //loadProductsDocsCollectionFail();

            //Però si carreguem directament els objectes sense el _id
            // loadProductsObjectCollection();
            //I podem obtenir els objectes amb el _id creat al MongoDB
            //SelectAllProducts();

            //loadBooksDocsCollection();
            //SelectOneBook();

            //loadBooksObjectCollection();
            //SelectOneBook();


            //EA3: LINQ Queries
            //SelectAllProducts();
            //SelectOneProductA();
            //SelectOneProductB();
            //SelectOneBook();
            // SelectFirstAuthorFromBookA();
            //SelectFirstAuthorFromBookB();
            // SelectBooksByPageCount();
            //SelectBiggerBook();

            // LoadPeopleObjectCollection();
            // SelectOnePersonObject();

            //Si carreguem documents amb $date i PublishedDate2
            //loadBooks2DocsCollection();
            //Al passaro a objectes falla
            //SelectOneBook2();

            //Però si carrego el fitxer amb $date i i la classe PublishedDate2 com objectes
            //loadBooks2ObjectCollection();
            //Després si que puc recuperar els objectes
            //SelectOneBook2();
            //SelectFirstAuthorFromBook2();


            //EA4. Agregacions

            //loadBooks2ObjectCollection();
            //SelectAuthorsByBook2();
            //CountBooks2ByStatus();
            //SelectNumAuthorsByBook2();

            //SelectLowScoreStudent();

            //LoadRestaurantsCollection();

            //CountRestaurantsByCuisine();

            //CountGradesByRestaurant();

            //SelectMaxScoreByRestaurant();

            //SelectCuisineByBorough();




        }
        private static void GetAllDBs()
        {
            

            var dbClient = MongoLocalConnection.GetMongoClient();
            
            var dbList = dbClient.ListDatabases().ToList();
            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }

        }

        private static void GetCollections()
        {
            
            var database = MongoLocalConnection.GetDatabase("sample_training");

            var colList = database.ListCollections().ToList();
            Console.WriteLine("The list of collection on this database is: ");
            foreach (var col in colList)
            {
                Console.WriteLine(col);
            }
        }

        private static void SelectAllStudents()
        {
          

            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var studentDocuments = collection.Find(new BsonDocument()).ToList();

            foreach (var student in studentDocuments)
            {
                Console.WriteLine(student.ToString());
            }

        }

        private static void InsertOneStudent()
        {
           
            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var document = new BsonDocument
            {
                { "student_id", 9999923 },
                { "scores", new BsonArray
                {
                        new BsonDocument{ {"type", "exam"}, {"score", 88.12334193287023 } },
                        new BsonDocument{ {"type", "quiz"}, {"score", 74.92381029342834 } },
                        new BsonDocument{ {"type", "homework"}, {"score", 89.97929384290324 } },
                        new BsonDocument{ {"type", "homework"}, {"score", 82.12931030513218 } }
                    }
                },
                { "class_id", 480}
            };


            collection.InsertOne(document);

        }

        private static void SelectOneStudent()
        {
            

            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 9999923);
            var studentDocument = collection.Find(filter).FirstOrDefault();
            Console.WriteLine(studentDocument.ToString());

        }

     
        private static void SelectStudentFields()
        {
            
            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 9999923);
            var studentDocument = collection.Find(filter).FirstOrDefault();
            var id = studentDocument.GetElement("student_id");
            var scores = studentDocument.GetElement("scores");

            Console.WriteLine(id.ToString());
            Console.WriteLine(scores.ToString());

        }

        private static void LoadPeopleCollection()
        {
            FileInfo file = new FileInfo("../../../files/people.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Person> people = JsonConvert.DeserializeObject<List<Person>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("people");

            var collection = database.GetCollection<BsonDocument>("people");

          

            if (people != null)
                foreach (var person in people)
                {
                    Console.WriteLine(person.name);
                    string json = JsonConvert.SerializeObject(person);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
        }

        private static void SelectPeopleByFriend()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

          
            var friendFilter1 = Builders<BsonDocument>.Filter.Eq("friends.name", "Serenity Watson");

            var people = collection.Find(friendFilter1).ToList();

            foreach (var person in people)
            {
                Console.WriteLine(person.ToString());
            }

            Console.WriteLine();

            var friendFilter2 = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>(
                "friends", new BsonDocument { { "name", "Rachel Hancock" } });

            var cursor = collection.Find(friendFilter2).ToCursor();
            foreach (var document in cursor.ToEnumerable())
            {
                Console.WriteLine(document.ToString());
                Console.WriteLine();

            }
        }
        private static void SelectPeopleByAge()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var ageFilter = Builders<BsonDocument>.Filter.Gt("age", 38);
            //var cursor = collection.Find(ageFilter).ToCursor();

            var sort = Builders<BsonDocument>.Sort.Descending("age");
            var cursor = collection.Find(ageFilter).Sort(sort).ToCursor();

            foreach (var document in cursor.ToEnumerable())
            {
                Console.WriteLine(document.ToString());
                Console.WriteLine();

            }

        }

        private static void UpdateOnePerson()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("name", "Sophie Gibbs");
            var update = Builders<BsonDocument>.Update.Set("isActive", false);

            var personDoc1 = collection.Find(filter).First();

            var name1 = personDoc1.GetElement("name");
            var isActive1 = personDoc1.GetElement("isActive");

            Console.WriteLine(name1.ToString() + "  " + isActive1.ToString());
            collection.UpdateOne(filter, update);

            var personDoc2 = collection.Find(filter).First();

            var name2 = personDoc2.GetElement("name");
            var isActive2 = personDoc2.GetElement("isActive");

            Console.WriteLine(name2.ToString() + "  " + isActive2.ToString());

        }

        private static void UpdateManyPeople()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("randomArrayItem", "teacher") & Builders<BsonDocument>
                .Filter.Eq("gender", "male");
            var update = Builders<BsonDocument>.Update.Set("company", "Treson");

            var docsUpdated = collection.UpdateMany(filter, update);
            Console.WriteLine("Docs modificats: " + docsUpdated.ModifiedCount);
            Console.WriteLine();

            var cursor = collection.Find(filter).ToCursor();

            foreach (var doc in cursor.ToEnumerable())
            {
                Console.WriteLine(doc.GetElement("company"));
                Console.WriteLine();

            }
        }

        private static void UpdatePeopleArrayPopLast()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("name", "Sophie Gibbs");
            var update = Builders<BsonDocument>.Update.PopLast("tags");

            var personDoc1 = collection.Find(filter).First();
            var tags1 = personDoc1.GetElement("tags");
            Console.WriteLine(tags1.ToString());

            collection.UpdateOne(filter, update);

            var personDoc2 = collection.Find(filter).First();
            var tags2 = personDoc2.GetElement("tags");
            Console.WriteLine(tags2.ToString());

        }

        private static void UpdatePeopleNewField()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("name", "Madelyn Murphy");
            var update = Builders<BsonDocument>.Update.Set("phone_aux", "666-470-34822");

            collection.UpdateOne(filter, update);

            var personDoc = collection.Find(filter).First();
            Console.WriteLine(personDoc.ToString());

        }

        private static void DeleteOnePerson()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var deleteFilter = Builders<BsonDocument>.Filter.Eq("name", "Eva Watson");

            var docsDeleted = collection.DeleteOne(deleteFilter);
            Console.WriteLine("Docs eliminats: " + docsDeleted.DeletedCount);

        }

        private static void DeleteManyPeople()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var deleteFilter = Builders<BsonDocument>.Filter.Lt("age", 23);

            var docsDeleted = collection.DeleteMany(deleteFilter);
            Console.WriteLine("Docs eliminats: " + docsDeleted.DeletedCount);

        }

        private static void DropCollection(String db, String col)
        {

            var database = MongoLocalConnection.GetDatabase(db);

            database.DropCollection(col);

            //Utilitzant un cursor

            var cursor = database.ListCollectionNames();

            foreach (var colName in cursor.ToEnumerable())
            {
                Console.WriteLine(colName.ToString());

            }

            //O utilitzant la llista
            /*
            var cols =  database.ListCollections().ToList();
            foreach (var col in cols)
            {
                Console.WriteLine(col.GetElement("name"));
            }
            */

        }

        private static void loadProductsDocsCollectionFail()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("products");
            var collection = database.GetCollection<BsonDocument>("products");

            FileInfo file = new FileInfo("../../../files/products.json");

            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Product product = JsonConvert.DeserializeObject<Product>(line);
                    Console.WriteLine(product.name);
                    string json = JsonConvert.SerializeObject(product);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
            }

        }

        private static void loadProducts2DocsCollection()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("products");
            var collection = database.GetCollection<BsonDocument>("products");

            FileInfo file = new FileInfo("../../../files/products2.json");

            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Product2 product = JsonConvert.DeserializeObject<Product2>(line);
                    Console.WriteLine(product.name);
                    string json = JsonConvert.SerializeObject(product);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
            }

        }

        private static void loadProductsObjectCollection()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("products");
            var collection = database.GetCollection<Product>("products");

            FileInfo file = new FileInfo("../../../files/products.json");

            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Product product = JsonConvert.DeserializeObject<Product>(line);
                    Console.WriteLine(product.name);
                    //string json = JsonConvert.SerializeObject(product);
                    //var document = new BsonDocument();
                    //document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(product);
                }
            }

        }

        private static void loadBooksDocsCollection()
        {
            FileInfo file = new FileInfo("../../../files/books.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");

            database.DropCollection("books");

            var collection = database.GetCollection<BsonDocument>("books");

            if (books != null)
                foreach (var book in books)
                {
                    Console.WriteLine(book.title);
                    string json = JsonConvert.SerializeObject(book);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
        }


        private static void loadBooksObjectCollection()
        {
            FileInfo file = new FileInfo("../../../files/books.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");

            database.DropCollection("books");

            var collection = database.GetCollection<Book>("books");

            if (books != null)
                foreach (var book in books)
                {
                    Console.WriteLine(book.title);
                    //string json = JsonConvert.SerializeObject(book);
                    //var document = new BsonDocument();
                    //document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(book);
                }
        }

        private static void loadBooks2DocsCollection()
        {
            FileInfo file = new FileInfo("../../../files/books2.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Book2> books = JsonConvert.DeserializeObject<List<Book2>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");

            database.DropCollection("books");

            var collection = database.GetCollection<BsonDocument>("books");

            if (books != null)
                foreach (var book in books)
                {
                    Console.WriteLine(book.title);
                    string json = JsonConvert.SerializeObject(book);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
        }


        private static void loadBooks2ObjectCollection()
        {
            FileInfo file = new FileInfo("../../../files/books2.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Book2> books = JsonConvert.DeserializeObject<List<Book2>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");

            database.DropCollection("books");

            var collection = database.GetCollection<Book2>("books");

            if (books != null)
                foreach (var book2 in books)
                {
                    Console.WriteLine(book2.title);
                   // string json = JsonConvert.SerializeObject(book);
                   // var document = new BsonDocument();
                   // document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(book2);
                }
        }

        private static void SelectAllProducts()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var productsCollection = database.GetCollection<Product>("products");

            var numProductes =
                productsCollection.AsQueryable<Product>()
                    .Count();

            Console.WriteLine("Número de productes:{0} ", numProductes);
            Console.WriteLine();


            var productList = productsCollection.AsQueryable<Product>().ToList();

            foreach (var product in productList)
            {
                Console.WriteLine("Nom :{0} " + "Id :{1} ", product.name, product._id);
            }
        }

        private static void SelectAllProducts2()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var productsCollection = database.GetCollection<Product2>("products");

            var numProductes =
                productsCollection.AsQueryable<Product2>()
                    .Count();

            Console.WriteLine("Número de productes:{0} ", numProductes);
            Console.WriteLine();


            var productList = productsCollection.AsQueryable<Product2>().ToList();

            foreach (var product in productList)
            {
              Console.WriteLine("Nom :{0} " + "Id :{1} ", product.name, product._id);
            }
        }


        private static void SelectOneProductA()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var productsCollection = database.GetCollection<Product>("products");

            var query =
                from p in productsCollection.AsQueryable<Product>()
                where p.name == "MacBook"
                select p;

            foreach (var product in query)
            {
                Console.WriteLine("Nom :{0} " + "Preu :{1} ", product.name, product.price);
            }
        }

        private static void SelectOneProductB()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var productsCollection = database.GetCollection<Product>("products");

            var query =
                productsCollection.AsQueryable<Product>()
                    .Where(p => p.name == "MacBook");

            foreach (var product in query)
            {
                Console.WriteLine("Nom :{0} ", product.name);

                foreach (var cat in product.categories)
                {
                    Console.WriteLine("Categoria :{0} ", cat);
                }
            }
        }


        private static void SelectOneBook()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book>("books");


            var query =
                from b in booksCollection.AsQueryable<Book>()
                where b.title == "Flexible Rails"
                select b;

            foreach (var book in query)
            {
                Console.WriteLine("Nom :{0} " + "Data publicació :{1} ", book.title, book.publishedDate);
            }

        }


        private static void SelectOneBook2()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book2>("books");


            var query =
                from b in booksCollection.AsQueryable<Book2>()
                where b.title == "Flexible Rails"
                select b;

            foreach (var book2 in query)
            {
                Console.WriteLine("Nom :{0} " + "Data publicació :{1} ", book2.title, book2.publishedDate);
            }

        }

        private static void SelectFirstAuthorFromBookA()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book>("books");

            var autor =
                booksCollection.AsQueryable<Book>()
                    .Where(b => b.title == "Flexible Rails")
                    .Select(b => b.authors[0])
                    .First();


            Console.WriteLine(autor);


        }

        private static void SelectFirstAuthorFromBookB()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book>("books");

            var autor =
                booksCollection.AsQueryable<Book>()
                    .Where(b => b.title == "Flexible Rails")
                    .Select(b => b.authors.First())
                    .First();


            Console.WriteLine(autor);


        }

        private static void SelectBooksByPageCount()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book>("books");

            var query =
                booksCollection.AsQueryable<Book>()
                    .Where(b => b.pageCount > 900)
                    .OrderBy(b => b.title);

            foreach (var book in query)
            {
                Console.WriteLine(book.title);
            }

        }

        private static void SelectBiggerBook()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book>("books");

            var query = booksCollection.AsQueryable<Book>();

            var maxPages =
                query
                    .Select(b => b.pageCount)
                    .Max();

            var book =
                query
                    .Where(b => b.pageCount == maxPages)
                    .Single();

            Console.WriteLine("El llibre amb més pàgines es diu :{0} " + "Número de pàgines :{1} ", book.title, book.pageCount);

        }

        private static void LoadPeopleObjectCollection()
        {
            FileInfo file = new FileInfo("../../../files/people.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Person2> people = JsonConvert.DeserializeObject<List<Person2>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("people");

            var collection = database.GetCollection<Person2>("people");

            if (people != null)
                foreach (var person2 in people)
                {
                    Console.WriteLine(person2.name);
                    //string json = JsonConvert.SerializeObject(person);
                    //var document = new BsonDocument();
                    //document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(person2);
                }
        }
        private static void SelectOnePersonObject()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var peopleCollection = database.GetCollection<Person2>("people");

            var query =
                from p in peopleCollection.AsQueryable<Person2>()
                where p.name == "Brooke Stanley"
                select p;

            foreach (var person in query)
            {
                Console.WriteLine("Nom :{0} " + "Telèfon :{1} ", person.name,  person.phone);
            }

        }
               

        //Agregacions. Mostra els autors d'un llibre
        private static void SelectAuthorsByBook2()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book2>("books");

            var aggregate = booksCollection.Aggregate()
                .Match(new BsonDocument { { "title", "iPhone in Action" } });
            var results = aggregate.ToList();
            foreach (var book in results)
            {
                Console.WriteLine(book.title);
                foreach (var auhor in book.authors)
                {
                    Console.WriteLine(auhor);
                }

            }
        }

        //Agregacions. Mostra quants llibres hi ha per "status"
        private static void CountBooks2ByStatus()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book2>("books");

            var aggregate = booksCollection.Aggregate()
                .Group(new BsonDocument { { "_id", "$status" }, { "count", new BsonDocument("$sum", 1) } })
                .Sort(new BsonDocument { { "count", -1 } });
            var results = aggregate.ToList();
            foreach (var obj in results)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        //Agregacions. Compta quants autors té cada llibre i els ordenes de forma descendent per 
        //número d'autors i mostra només els tres llibres amb més autors
        private static void SelectNumAuthorsByBook2()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var booksCollection = database.GetCollection<Book2>("books");
            var aggregate = booksCollection.Aggregate()
                .Unwind("authors")
                .Group(new BsonDocument { { "_id", "$title" }, { "numAutors", new BsonDocument("$sum", 1) } })
                .Sort(new BsonDocument { { "numAutors", -1 } })
                .Limit(3);

            var results = aggregate.ToList();
            foreach (var obj in results)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        //Agregacions. Mostra la nota més baixa per cada estudiant
        private static void SelectLowScoreStudent()
        {
            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");
            var aggregate = collection.Aggregate()
                .Unwind("scores")
                .Group(new BsonDocument { { "_id", "$_id" }, { "lowscore", new BsonDocument("$min", "$scores.score") } });

            var results = aggregate.ToList();
            foreach (var obj in results)
            {
                Console.WriteLine(obj.ToString());
            }
        }


        //load restaurants collection

        public static void LoadRestaurantsCollection()
        {
            FileInfo file = new FileInfo("../../../files/restaurants.json");
            StreamReader sr = file.OpenText();

            List<Restaurant> restaurants = new List<Restaurant>();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Restaurant restaurant = JsonConvert.DeserializeObject<Restaurant>(line);
                restaurants.Add(restaurant);
            }

            sr.Close();

            var database = MongoLocalConnection.GetDatabase("ATAQUEMALMONGO");
            var collection = database.GetCollection<BsonDocument>("RESTAURANT2");

            if (restaurants.Count > 0)
            {
                foreach (var restaurant in restaurants)
                {
                    Console.WriteLine(restaurant.name);
                    string json = JsonConvert.SerializeObject(restaurant);
                    var document = BsonDocument.Parse(json);
                    collection.InsertOne(document);
                }
                Console.WriteLine("Restaurants collection loaded");
            }
        }

        public static void CountRestaurantsByCuisine()
        {
            var database = MongoLocalConnection.GetDatabase("ATAQUEMALMONGO");
            var collection = database.GetCollection<BsonDocument>("RESTAURANT2");

            var aggregate = collection.Aggregate()
                .Group(new BsonDocument { { "_id", "$cuisine" }, { "count", new BsonDocument("$sum", 1) } })
                .Sort(new BsonDocument { { "count", 1 } });

            var results = aggregate.ToList();
            foreach (var obj in results)
            {
                Console.WriteLine(obj.ToString());
            }
        }



        public static void CountGradesByRestaurant()
        {
            var database = MongoLocalConnection.GetDatabase("ATAQUEMALMONGO");
            var collection = database.GetCollection<BsonDocument>("RESTAURANT2");

            var aggregate = collection.Aggregate()
                .Unwind("grades")
                .Group(new BsonDocument { { "_id", "$name" }, { "count", new BsonDocument("$sum", 1) } });

            var results = aggregate.ToList();
            foreach (var obj in results)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static void SelectMaxScoreByRestaurant()
        {
            var database = MongoLocalConnection.GetDatabase("ATAQUEMALMONGO");
            var collection = database.GetCollection<BsonDocument>("RESTAURANT2");

            var aggregate = collection.Aggregate()
                .Unwind("grades")
                .Group(new BsonDocument { { "_id", "$name" }, { "maxScore", new BsonDocument("$max", "$grades.score") } });

            var results = aggregate.ToList();
            foreach (var obj in results)
            {
                Console.WriteLine(obj.ToString());
            }
        }

       

        public static void SelectCuisineByBorough()
        {
            var database = MongoLocalConnection.GetDatabase("ATAQUEMALMONGO");
            var collection = database.GetCollection<BsonDocument>("RESTAURANT2");

            var aggregate = collection.Aggregate()
                .Group(new BsonDocument { { "_id", "$borough" }, { "cuisine", new BsonDocument("$addToSet", "$cuisine") } });

            var results = aggregate.ToList();
            foreach (var obj in results)
            {
                Console.WriteLine(obj.ToString());
            }
        }


    }
}