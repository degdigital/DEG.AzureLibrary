using System;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Class GenericFiber.
    /// </summary>
    public class GenericFiber
    {
        int _cursorAt;

        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        /// <value>The cursor.</value>
        public object Cursor { get; set; }
        /// <summary>
        /// Gets or sets the cursor at.
        /// </summary>
        /// <value>The cursor at.</value>
        public int CursorAt
        {
            get => _cursorAt;
            set => PendingAt = _cursorAt = value;
        }
        /// <summary>
        /// Gets or sets the pending at.
        /// </summary>
        /// <value>The pending at.</value>
        public int PendingAt { get; set; }
        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit() { CursorAt = PendingAt; OnCommit?.Invoke(Tag); }
        /// <summary>
        /// Gets or sets the on commit.
        /// </summary>
        /// <value>The on commit.</value>
        public Action<object> OnCommit { get; set; }
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }
        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        public string Note { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Cursor?.ToString() ?? "null"}:{CursorAt}{(Note != null ? "." + Note : string.Empty)}";
        }
    }
}
