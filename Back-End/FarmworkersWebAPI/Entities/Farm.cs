namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using FluentValidation;

    [Table("Farm")]
    public partial class Farm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public Farm()
        {
            UserFarms = new HashSet<UserFarm>();
            WeatherHistories = new HashSet<WeatherHistory>();
        }

        public int FarmID { get; set; }

        [Required]
        public string FarmName { get; set; }

        [Required]
        public string FarmHouseNumberStreetAddress { get; set; }

        [Required]
        public string FarmCity { get; set; }

        [Required]
        public string FarmState { get; set; }

        [Required]
        public string FarmCountry { get; set; }

        public int FarmZipCode { get; set; }

        [Required]
        public string FarmLatitute { get; set; }

        [Required]
        public string FarmLongitude { get; set; }

        [Required]
        public string FarmTemperatureMin { get; set; }

        [Required]
        public string FarmTemperatureMax { get; set; }

        [StringLength(1)]
        public string IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFarm> UserFarms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeatherHistory> WeatherHistories { get; set; }

        public class FarmValidator : AbstractValidator<Farm>
        {
            public FarmValidator(string _actionType)
            {

                if (_actionType == "Update")
                {
                    RuleFor(farm => farm.FarmID.ToString()).NotEmpty().
                                               WithMessage("The Farm ID Must NOT be Empty or Null").
                                               Matches("([0-9]){1,9}").
                                               WithMessage("The Farm ID should only contain numbers").
                                               Length(5).
                                               WithMessage("The Farm ID  should contain a maximum of 9 Digits");
                }

                RuleFor(farm => farm.FarmName).NotEmpty().
                                               WithMessage("The Farm Name Must NOT be Empty or Null").
                                               Matches(@"([a-zA-Z_À-ÿ]|[ ]|[´]|[.]|[,]|[0-9]){1,200}").
                                               WithMessage("Farm Name should only contain letters and numbers").
                                               Length(1, 200).
                                               WithMessage("Farm Name should contain at least 1 character and maximum 200");

                RuleFor(farm => farm.FarmHouseNumberStreetAddress).NotEmpty().
                                               WithMessage("The Farm Address Must NOT be Empty or Null").
                                               Matches(@"([a-zA-Z_À-ÿ]|[ ]|[´]|[.]|[,]|[0-9]){1,250}").
                                               WithMessage("Farm Address should only contain letters and numbers").
                                               Length(1, 250).
                                               WithMessage("Farm Name should contain at least 1 character and maximum 250");

                RuleFor(farm => farm.FarmCity).NotEmpty().
                                               WithMessage("The Farm City Must NOT be Empty or Null").
                                               Matches(@"([a-zA-Z_À-ÿ]|[ ]){1,50}").
                                               WithMessage("The Farm City should only contain letters").
                                               Length(1, 50).
                                               WithMessage("Farm City should contain at least 1 character and maximum 50");

                RuleFor(farm => farm.FarmState).NotEmpty().
                                               WithMessage("The Farm State Must NOT be Empty or Null").
                                               Matches(@"([a-zA-Z_À-ÿ]|[ ]){1,50}").
                                               WithMessage("The Farm State should only contain letters").
                                               Length(1, 50).
                                               WithMessage("The Farm State should contain at least 1 character and maximum 50");

                RuleFor(farm => farm.FarmCountry).NotEmpty().
                                               WithMessage("The Farm Country Must NOT be Empty or Null").
                                               Matches(@"([a-zA-Z_À-ÿ]|[ ]){1,50}").
                                               WithMessage("The Farm Country should only contain letters").
                                               Length(1, 50).
                                               WithMessage("The Farm Country should contain at least 1 character and maximum 50");

                RuleFor(farm => farm.FarmZipCode.ToString()).NotEmpty().
                                               WithMessage("The Farm Zip Code Must NOT be Empty or Null").
                                               Matches("([0-9]){1,5}").
                                               WithMessage("The Farm Zip Code should only contain numbers").
                                               Length(5).
                                               WithMessage("The Farm Zip Code should contain EXACTLY 5 Digits");

                RuleFor(farm => farm.FarmLatitute).NotEmpty().
                                               WithMessage("The Farm Latitude Must NOT be Empty or Null").
                                               Matches(@"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$").
                                               WithMessage("The Farm Latitude Code should only contain numbers. And make sure it is valid").
                                               Length(1, 20).
                                               WithMessage("The Farm Latitude should contain at least 1 character and maximum 20");

                RuleFor(farm => farm.FarmLongitude).NotEmpty().
                                   WithMessage("The Farm Longitude Must NOT be Empty or Null").
                                   Matches(@"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$").
                                   WithMessage("The Farm Longitude Code should only contain numbers. And make sure it is valid").
                                   Length(1, 20).
                                   WithMessage("The Farm Longitude should contain at least 1 character and maximum 20");

                RuleFor(farm => farm.FarmTemperatureMin).NotEmpty().
                                               WithMessage("The Farm Minimum Temperature Must NOT be Empty or Null").
                                               Matches("([0-9]){1,3}").
                                               WithMessage("The Farm Minimum Temperature should only numbers and must be an integer").
                                               Length(1, 3).
                                               WithMessage("The Farm Farm Minimum Temperature should contain at least 1 character and maximum 3");

                RuleFor(farm => farm.FarmTemperatureMax).NotEmpty().
                                               WithMessage("The Farm Maximum Temperature Must NOT be Empty or Null").
                                               Matches("([0-9]){1,3}").
                                               WithMessage("The Farm Maximum Temperature should only numbers and must be an integer").
                                               Length(1, 3).
                                               WithMessage("The Farm Farm Maximum Temperature should contain at least 1 character and maximum 3");

                RuleFor(farm => farm.IsActive).NotEmpty().
                                               WithMessage("The Farm Is Active Attribute Must NOT be Empty or Null").
                                               Matches("[01]").
                                               WithMessage("The Farm Is Active Attribute should only be 0 (Zero) or 1 (One)").
                                               Length(1).
                                               WithMessage("The Farm Is Active Attribute should contain exactly ONE character");

              //Farm Owner Name and Email was Removed from this validation
              //That information will be validated in the USER Table

            }

            private bool BeAValidPostcode(string postcode)
            {
                // custom postcode validating logic goes here

                return true;
            }
        }
    }
}
