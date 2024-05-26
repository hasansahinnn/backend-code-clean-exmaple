namespace Service.Constants;

/// <summary>
/// Contains all the messages used in the application.
/// This class is used to avoid magic strings in the code.
/// </summary>
public static class Messages
{
    #region Validation Messages

    public const string IdCannotBeNull = "Id cannot be null!";
    public const string UserEmailCannotBeNull = "UserEmail cannot be null!";
    public const string YouMustEnterAValidEmail = "You must enter a valid email!";
    public const string IdMustBeGreaterThanZero = "Id must be greater than zero!";
    public const string StatusMustBeGreaterThanZero = "Status must be greater than zero!";
    public const string ContentCannotBeNull = "Content cannot be null!";
    public const string ContentCannotBeEmpty = "Content cannot be empty!";

    #endregion

    #region User Messages

    public const string UserNotFound = "User not found!";
    public const string OnlyUsersCanCreatePost = "Only users can create post!";
    public const string UserDeletedSuccessfully = "User deleted successfully.";
    public const string UserEmailUpdatedSuccessfully = "User email updated successfully.";
    public const string InvalidEmail = "Invalid email";

    #endregion

    #region Post Messages

    public const string InvalidPostStatusValue = "Invalid status value";
    public const string PostCreatedSuccessfully = "Post created successfully.";
    public const string PostCannotBeNull = "Post cannot be null!";
    public const string PostDeleted = "Post deleted.";
    public const string PostPublishedSuccessfully = "Post published successfully.";
    public const string PostNotFound = "Post not found!";
    public const string CommentAddSuccessfully = "Comment added successfully.";

    #endregion

    #region Comment Messages

    public const string CommentTextCannotBeNull = "Comment text cannot be null!";
    public const string CommentTextCannotBeEmpty = "Comment text cannot be empty!";

    #endregion

    #region Library Messages

    public const string MemberNotFound = "Member not found!";
    public const string BookNotFound = "Book not found!";
    public const string BookIdMustBeEntered = "Book ID must be entered";
    public const string DaysAgoMustBeZeroOrGreater = "Previous days number must be zero or greater than zero.";
    public const string BooksBorrowedAndEmailSentSuccessfully = "Books borrowed and email sent successfully.";
    public const string NoBorrowRecordGroupsFound = "No borrow record groups found for the given dates.";
    public const string BorrowEmailSubject = "Books Borrowed";
    public const string BorrowEmailTextBodyTemplate = "You have borrowed the following books. Use this Permission Code for Confirmation --> 1421:\n{0}";
    public const string RecentBorrowersEmailSubject = "Recent Book Borrowers Report";
    #endregion


}