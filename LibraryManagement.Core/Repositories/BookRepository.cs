using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Repositories;

public class BookRepository
{
    private readonly List<Book> _books;

    public BookRepository()
    {
        _books = new List<Book>();
    }

    public IReadOnlyCollection<Book> GetBooks()
    {
        return _books;
    }

    public Book? FindBook(string title, string authorName)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Book title is empty", nameof(title));

        if (string.IsNullOrWhiteSpace(authorName))
            throw new ArgumentException("Book author name is empty", nameof(authorName));

        return _books.FirstOrDefault(
            book => book.Title.Equals(title, StringComparison.Ordinal)
                    && book.AuthorName.Equals(authorName, StringComparison.Ordinal));
    }

    public void AddBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        if (FindBook(book.Title, book.AuthorName) is not null)
            throw new LibraryException("Book with this title and author already exists");

        _books.Add(book);
    }
}
