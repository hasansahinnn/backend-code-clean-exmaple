namespace Core.Constants;

public static class ResponseDescriptions
{
    #region Books Controller

    public const string CreateBorrow = "Creates a new borrow record for a user.";
    public const string GetRecentBorrowers = "Gets a list of recent borrowers within the specified number of days.";
    public const string SendEmailsForRecentBorrowers = "Sends emails to recent borrowers within the specified number of days.";

    #endregion

    #region Comments Controller

    public const string CreateComment = "Creates a new comment for a post.";

    #endregion

    #region PostsController

    public const string CreatePost = "Creates a new post.";
    public const string DeletePost = "Deletes a post by its ID.";
    public const string PublishPost = "Publishes a post.";
    public const string AddCommentAndArchiveOldPosts = "Adds a comment to a post and archives old posts.";
    public const string GetPostsAndUsers = "Gets all posts along with their associated users.";

    #endregion


}