using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ComparaBooks
{
    public class Gestor
    {
        private List<ArchivoAward> _lsArchivoSigla;
        private List<string> _lsError;
        private List<Book> _lsBookFinal;

        public void CreaArchivoFinal(string _pathBase)
        {
            _lsArchivoSigla = Book.GetArchivoAward();

            if (GeneraBooksIniciales(_pathBase))
            {
                GeneraListadoFinal();
                GeneraArchivoFinal(_pathBase);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathBase"></param>
        private void GeneraArchivoFinal(string pathBase)
        {
            if (_lsBookFinal.Count == 0)
            {
                Console.WriteLine("ERROR: Archivo vacío");
                return;
            }

            string pathIni = pathBase + "\\02\\CIFI.txt";

            using (StreamWriter sw = new StreamWriter(pathIni))
            {
                foreach (Book book in _lsBookFinal)
                {
                    if (book.Titulo != "*" && book.TituloAlt != "*")
                    {
                        string line = book.Anno
                            + "\t"
                            + book.Titulo
                            + (book.TituloAlt != string.Empty ? " [aka " + book.TituloAlt + "]" : "")
                            + "\t"
                            + book.Autor
                            + "\t"
                            + (book.Ganadora ? "Si" : "No")
                            + "\t"
                            + "" // Español
                            + "\t"
                            + "" // Saga
                            + "\t"
                            + ""; // Tipo

                        foreach (Award awardLine in book.Awards)
                        {
                            switch(awardLine.TipoPremio)
                            {
                                case Award.TipoAward.Nada:
                                    line += "\t" + "";
                                    break;
                                case Award.TipoAward.Ganadora:
                                    line += "\t" + "Si";
                                    break;
                                case Award.TipoAward.Nominada:
                                    line += "\t" + "No";
                                    break;
                            }
                        }

                        sw.WriteLine(line);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathBase"></param>
        private void GeneraListadoFinal()
        {
            _lsBookFinal = new List<Book>();

            foreach (ArchivoAward archivoAwardCurrent in _lsArchivoSigla)
            {
                int posicion = archivoAwardCurrent.Posicion;
                string siglaPremio = archivoAwardCurrent.SiglaPremio;

                foreach (Book bookCurrent in archivoAwardCurrent.Books)
                {
                    if (SiNoExisteEnListadoFinal(bookCurrent))
                    {
                        Book bookFinal = new Book()
                        {
                            Anno = bookCurrent.Anno,
                            Titulo = bookCurrent.Titulo,
                            TituloAlt = bookCurrent.TituloAlt,
                            Autor = bookCurrent.Autor,
                            Ganadora = bookCurrent.Ganadora
                        };

                        // Marca la casilla True o Falsa con el premio correspondiente
                        bookFinal.Awards.FirstOrDefault(nodo => nodo.Premio == siglaPremio).TipoPremio = bookCurrent.Ganadora ? Award.TipoAward.Ganadora : Award.TipoAward.Nominada;

                        // Recorre las otras listas para encontrar coincidencias
                        foreach (ArchivoAward archivoAwardRevision in _lsArchivoSigla)
                        {
                            string siglaPremioRevision = archivoAwardRevision.SiglaPremio;

                            if (archivoAwardRevision.Posicion != posicion)
                            {
                                foreach (Book bookRevision in archivoAwardRevision.Books)
                                {
                                    if (EsMismoLibro(bookFinal, bookRevision))
                                    {
                                        if (!bookFinal.Ganadora)
                                            bookFinal.Ganadora = bookRevision.Ganadora;

                                        if (bookFinal.TituloAlt == string.Empty
                                            && bookRevision.TituloAlt != string.Empty)
                                        {
                                            if (bookFinal.Titulo != bookRevision.TituloAlt)
                                                bookFinal.TituloAlt = bookRevision.TituloAlt;
                                            else
                                                bookFinal.TituloAlt = bookRevision.Titulo;
                                        }

                                        bookFinal.Awards.FirstOrDefault(nodo => nodo.Premio == siglaPremioRevision).TipoPremio = bookRevision.Ganadora ? Award.TipoAward.Ganadora : Award.TipoAward.Nominada;
                                    }
                                }
                            }
                        }

                        _lsBookFinal.Add(bookFinal);
                    }
                }
            }
        }

        /// <summary>
        /// Busca si ya se encuentra el book en el listado final
        /// </summary>
        /// <param name="bookCurrent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool SiNoExisteEnListadoFinal(Book bookCurrent)
        {
            string titulo = bookCurrent.Titulo.ToLower();
            string tituloAlt = bookCurrent.TituloAlt != string.Empty ? bookCurrent.TituloAlt.ToLower() : bookCurrent.Titulo.ToLower();
            string autor = bookCurrent.Autor.ToLower();

            return
                _lsBookFinal.Where(nodo => nodo.Titulo.ToLower() == titulo && nodo.Autor.ToLower() == autor).Count() == 0
                && _lsBookFinal.Where(nodo => nodo.TituloAlt.ToLower() == titulo && nodo.Autor.ToLower() == autor).Count() == 0
                && _lsBookFinal.Where(nodo => nodo.Titulo.ToLower() == tituloAlt && nodo.Autor.ToLower() == autor).Count() == 0
                && _lsBookFinal.Where(nodo => nodo.TituloAlt.ToLower() == tituloAlt && nodo.Autor.ToLower() == autor).Count() == 0;
        }

        /// <summary>
        /// Chequea si son el mismo libro (Titulo y Autor)
        /// </summary>
        /// <param name="BookFinal"></param>
        /// <param name="bookTemporal"></param>
        /// <returns></returns>
        private bool EsMismoLibro(Book bookFinal, Book bookRevision)
        {
            string tituloFinal = bookFinal.Titulo.ToLower();
            string tituloAltFinal = bookFinal.TituloAlt != string.Empty ? bookFinal.TituloAlt.ToLower() : bookFinal.Titulo.ToLower();
            string autorFinal = bookFinal.Autor.ToLower();

            string tituloRevision = bookRevision.Titulo.ToLower();
            string tituloAltRevision = bookRevision.TituloAlt != string.Empty ? bookRevision.TituloAlt.ToLower() : bookRevision.Titulo.ToLower();
            string autorRevision = bookRevision.Autor.ToLower();

            return
                (tituloFinal == tituloRevision || tituloAltFinal == tituloRevision || tituloFinal == tituloAltRevision || tituloAltFinal == tituloAltRevision)
                && autorFinal == autorRevision;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathBase"></param>
        private bool GeneraBooksIniciales(string pathBase)
        {
            _lsError = new List<string>();
            string pathIni = pathBase + "\\01";

            foreach (string fileAward in Directory.GetFiles(pathIni))
            {
                ArchivoAward archivoAward = GetArchivoAward(fileAward);

                if (archivoAward == null)
                    throw new Exception("No se encontró la sigla para " + fileAward);

                List<Book> lsBook = LeeArchivo(fileAward);
                if (_lsError.Count > 0)
                {
                    foreach (string error in _lsError)
                    {
                        Console.WriteLine(error);
                    }
                    return false;
                }

                archivoAward.Books = lsBook;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileAward"></param>
        /// <param name="archivoSigla"></param>
        /// <returns></returns>
        private List<Book> LeeArchivo(string fileAward)
        {
            string fileError = Path.GetFileNameWithoutExtension(fileAward);
            List<Book> lsBooks = new List<Book>();

            using (StreamReader sr = new StreamReader(fileAward))
            {
                int annioActual = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    if (!EsAnnio(line, ref annioActual))
                    {
                        if (GeneraBookCorrecto(fileError, annioActual, line, out Book book))
                        {
                            book.Anno = annioActual;
                            lsBooks.Add(book);
                        }
                    }
                }
            }

            return lsBooks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private bool GeneraBookCorrecto(string fileAward, int annioActual, string line, out Book book)
        {
            book = new Book();

            if (line.Length == 0)
                return false;

            if (line.Substring(0, 9).ToLower() == "winner: *")
            {
                book.Ganadora = true;
                book.Titulo = "*";
                book.Autor = string.Empty;
                return true;
            }
            else if (line.Substring(0, 8).ToLower() == "winner: ")
            {
                line = line.Substring(8);
                book.Ganadora = true;
            }
            else
                book.Ganadora = false;

            string[] array = line.Split('(');
            if (array.Length > 1)
            {
                if (array.Length > 2)
                    _lsError.Add("ERROR: Paréntesis [" + fileAward + "](" + annioActual + "): " + line);
                else
                    line = array[0];
            }

            array = line.Split(',');
            if (array.Length != 2)
                _lsError.Add("ERROR: Coma [" + fileAward + "](" + annioActual + "): " + line);

            book.Titulo = array[0].Replace(".", ",").Replace("<", "(").Replace(">", ")").Replace(";",".");
            book.Autor = array[1].Trim();

            if (book.Titulo.Contains('['))
            {
                array = book.Titulo.Split('[');
                book.Titulo = array[0].Trim();
                book.TituloAlt = array[1].Replace("]", "").Trim();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="annioActual"></param>
        /// <returns></returns>
        private bool EsAnnio(string line, ref int annioActual)
        {
            if (line.Length > 4)
                return false;

            if (Int32.TryParse(line, out int annioTemp))
            {
                annioActual = annioTemp;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileAward"></param>
        /// <returns></returns>
        private ArchivoAward GetArchivoAward(string fileAward)
        {
            return _lsArchivoSigla.FirstOrDefault(nodo => nodo.NombreArchivo == Path.GetFileNameWithoutExtension(fileAward));
        }


    }
}
