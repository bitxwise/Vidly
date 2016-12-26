using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Data;

namespace Vidly.Models
{
    public class MemberAgeConstraint : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (ICustomerData)validationContext.ObjectInstance;
            
            if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
                return ValidationResult.Success;

            if (!customer.BirthDate.HasValue)
                return new ValidationResult("Birth Date is required.");

            var age = (DateTime.Today - customer.BirthDate.Value).TotalDays / 365;

            return age >= 18
                ? ValidationResult.Success
                : new ValidationResult("Customer must be at least 18 years of age to be a member.");
        }
    }
}