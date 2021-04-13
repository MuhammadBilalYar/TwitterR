namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("TwitterUser")]
    public class TwitterUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TwitterUserId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime BirthDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Email { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Phone { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Login { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Password { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Photo_URL { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PublicUserId { get; set; }
    }
}