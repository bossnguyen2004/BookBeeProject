﻿using System.ComponentModel.DataAnnotations;

namespace BookBee.Model
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? DateOfBirth { get; set; }
        public int? DeathYear { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public DateTime Update { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
