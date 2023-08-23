using System;
using System.Collections.Generic;
using System.Text;

namespace ComparaBooks
{
    public class Book
    {
        public int Anno = 0;
        public string Titulo = string.Empty;
        public string TituloAlt = string.Empty;
        public string Autor = string.Empty;
        public bool Ganadora = false;

        public string Espanol = string.Empty;
        public string Saga = string.Empty;
        public string Tipo = string.Empty;

        public List<Award> Awards = new List<Award>();

        /// <summary>
        /// 
        /// </summary>
        public static List<ArchivoAward> GetArchivoAward()
        {
            return new List<ArchivoAward>()
            {
                new ArchivoAward()
                {
                    NombreArchivo = "Hugo Awards",
                    SiglaPremio = "Hug",
                    Posicion = 1,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "Nebula Awards",
                    SiglaPremio = "Neb",
                    Posicion = 2,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "Locus Awards CIFI",
                    SiglaPremio = "LoC",
                    Posicion = 3,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "Locus Awards FAN",
                    SiglaPremio = "LoF",
                    Posicion = 4,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "British SF Association Awards",
                    SiglaPremio = "BrC",
                    Posicion = 5,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "British Fantasy Awards",
                    SiglaPremio = "BrF",
                    Posicion = 6,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "World Fantasy Awards",
                    SiglaPremio = "WFa",
                    Posicion = 7,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "John W Campbell Memorial Award",
                    SiglaPremio = "JCM",
                    Posicion = 8,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "Philip K Dick Award",
                    SiglaPremio = "PKD",
                    Posicion = 9,
                    Books = new List<Book>()
                }
                ,new ArchivoAward()
                {
                    NombreArchivo = "Arthur C Clarke Award",
                    SiglaPremio = "ACC",
                    Posicion = 10,
                    Books = new List<Book>()
                }
            };
        }

        public Book()
        {
            Awards = new List<Award>();
            foreach (ArchivoAward archivoAward in Book.GetArchivoAward())
            {
                Award Award = new Award()
                {
                    Premio = archivoAward.SiglaPremio,
                    Ganadora = false
                };
                Awards.Add(Award);
            }
        }

    }
}
