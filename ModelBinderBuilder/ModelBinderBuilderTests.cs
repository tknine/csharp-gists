using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinderBuilder
{
    [TestClass]
    public class ModelBinderBuilderTests
    {
        private ModelBinderBuilder<Movie> movieUpdateBinding = ModelBinderBuilder<Movie>.Get();

        [TestMethod]
        public void TestGetPropertyName()
        {
            string test = ModelBinderBuilder<Movie>.Get().GetProperty(x => x.Name);

            Assert.AreEqual("Name", test);
        }

        [TestMethod]
        public void TestAddMultipleProperties()
        {
            var test = ModelBinderBuilder<Movie>.Get()
                .Add(x => x.NumberAvailable)
                .Add(x => x.GenreId)
                .Add(x => x.Name)
                .Add(x => x.Id)
                .Add(x => x.NumberAvailable)
                .ToString();

            Assert.AreEqual("NumberAvailable,GenreId,Name,Id", test);
        }

        [TestMethod]
        public void TestCannotAddPropertyFromOtherType()
        {

            var g = new Genre();

            Action p = () =>
            {
                ModelBinderBuilder<Movie>.Get()
                     .Add(x => x.NumberAvailable)
                     .Add(x => g.GenreName);
            };

            Assert.ThrowsException<ArgumentException>(p);

        }

        [TestMethod]
        public void TestAll()
        {
            var test = ModelBinderBuilder<Movie>.Get()
                .All()
                .ToString();

            Assert.AreEqual("Id,Name,Genre,GenreId,DateAdded,ReleaseDate,NumberInStock,NumberAvailable", test);

        }

        [TestMethod]
        public void TestAllRemoveName()
        {
            var test = ModelBinderBuilder<Movie>.Get()
                .All()
                .Remove(x => x.Name)
                .ToString();

            Assert.AreEqual("Id,Genre,GenreId,DateAdded,ReleaseDate,NumberInStock,NumberAvailable", test);

        }
    }

    public class Movie
    {
        public static string MovieStaticValue = "Test";

        public int Id { get; set; }

        public string Name { get; set; }

        public Genre Genre { get; set; }

        public byte GenreId { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime ReleaseDate { get; set; }

        public byte NumberInStock { get; set; }

        public byte NumberAvailable { get; set; }
    }

    public class Genre
    {
        public static string GenreStaticValue = "Test";

        public byte Id { get; set; }

        public string GenreName { get; set; }
    }
}
