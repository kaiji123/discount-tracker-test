using System;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    public int CustomerId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    public DateTime RegistrationDate { get; set; }

    // Other properties, methods, and relationships can be added here
}
