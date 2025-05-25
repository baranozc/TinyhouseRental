using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int ListingID { get; set; }
        public int UserID { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsEdited { get; set; }
        public bool IsReported { get; set; }
        public List<Comment> Replies { get; set; }
        public int? ParentCommentID { get; set; }

        public Comment()
        {
            CreatedDate = DateTime.Now;
            Replies = new List<Comment>();
            LikeCount = 0;
            DislikeCount = 0;
            IsEdited = false;
            IsReported = false;
        }

        public void EditComment(string newContent)
        {
            CommentContent = newContent;
            LastEditedDate = DateTime.Now;
            IsEdited = true;
        }

        public void AddReply(Comment reply)
        {
            reply.ParentCommentID = this.CommentID;
            Replies.Add(reply);
        }

        public void RemoveReply(int replyId)
        {
            Replies.RemoveAll(r => r.CommentID == replyId);
        }

        public void Like()
        {
            LikeCount++;
        }

        public void Dislike()
        {
            DislikeCount++;
        }

        public void Report()
        {
            IsReported = true;
        }

        public void Unreport()
        {
            IsReported = false;
        }

        public double GetRating()
        {
            if (LikeCount + DislikeCount == 0)
                return 0;
            return (double)LikeCount / (LikeCount + DislikeCount) * 5;
        }

        public override string ToString()
        {
            return $"{CommentContent} - {CreatedDate:g}";
        }
    }
} 