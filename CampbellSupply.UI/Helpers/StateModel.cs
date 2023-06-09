using System.Collections.Generic;
using System.Linq;

namespace CampbellSupply.Helpers
{
    public class StateModel
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public List<StateModel> GetStates()
        {
            List<StateModel> states = new List<StateModel>();
            states.Add(new StateModel() { Abbreviation = "AL", Name = "Alabama" });
            states.Add(new StateModel() { Abbreviation = "AK", Name = "Alaska" });
            states.Add(new StateModel() { Abbreviation = "AR", Name = "Arkansas" });
            states.Add(new StateModel() { Abbreviation = "AZ", Name = "Arizona" });
            states.Add(new StateModel() { Abbreviation = "CA", Name = "California" });
            states.Add(new StateModel() { Abbreviation = "CO", Name = "Colorado" });
            states.Add(new StateModel() { Abbreviation = "CT", Name = "Connecticut" });
            states.Add(new StateModel() { Abbreviation = "DE", Name = "Delaware" });
            states.Add(new StateModel() { Abbreviation = "FL", Name = "Florida" });
            states.Add(new StateModel() { Abbreviation = "GA", Name = "Georgia" });
            states.Add(new StateModel() { Abbreviation = "HI", Name = "Hawaii" });
            states.Add(new StateModel() { Abbreviation = "ID", Name = "Idaho" });
            states.Add(new StateModel() { Abbreviation = "IL", Name = "Illinois" });
            states.Add(new StateModel() { Abbreviation = "IN", Name = "Indiana" });
            states.Add(new StateModel() { Abbreviation = "IA", Name = "Iowa" });
            states.Add(new StateModel() { Abbreviation = "KS", Name = "Kansas" });
            states.Add(new StateModel() { Abbreviation = "KY", Name = "Kentucky" });
            states.Add(new StateModel() { Abbreviation = "LA", Name = "Louisiana" });
            states.Add(new StateModel() { Abbreviation = "ME", Name = "Maine" });
            states.Add(new StateModel() { Abbreviation = "MD", Name = "Maryland" });
            states.Add(new StateModel() { Abbreviation = "MA", Name = "Massachusetts" });
            states.Add(new StateModel() { Abbreviation = "MI", Name = "Michigan" });
            states.Add(new StateModel() { Abbreviation = "MN", Name = "Minnesota" });
            states.Add(new StateModel() { Abbreviation = "MS", Name = "Mississippi" });
            states.Add(new StateModel() { Abbreviation = "MO", Name = "Missouri" });
            states.Add(new StateModel() { Abbreviation = "MT", Name = "Montana" });
            states.Add(new StateModel() { Abbreviation = "NE", Name = "Nebraska" });
            states.Add(new StateModel() { Abbreviation = "NH", Name = "New Hampshire" });
            states.Add(new StateModel() { Abbreviation = "NJ", Name = "New Jersey" });
            states.Add(new StateModel() { Abbreviation = "NM", Name = "New Mexico" });
            states.Add(new StateModel() { Abbreviation = "NY", Name = "New York" });
            states.Add(new StateModel() { Abbreviation = "NC", Name = "North Carolina" });
            states.Add(new StateModel() { Abbreviation = "NV", Name = "Nevada" });
            states.Add(new StateModel() { Abbreviation = "ND", Name = "North Dakota" });
            states.Add(new StateModel() { Abbreviation = "OH", Name = "Ohio" });
            states.Add(new StateModel() { Abbreviation = "OK", Name = "Oklahoma" });
            states.Add(new StateModel() { Abbreviation = "OR", Name = "Oregon" });
            states.Add(new StateModel() { Abbreviation = "PA", Name = "Pennsylvania" });
            states.Add(new StateModel() { Abbreviation = "RI", Name = "Rhode Island" });
            states.Add(new StateModel() { Abbreviation = "SC", Name = "South Carolina" });
            states.Add(new StateModel() { Abbreviation = "SD", Name = "South Dakota" });
            states.Add(new StateModel() { Abbreviation = "TN", Name = "Tennessee" });
            states.Add(new StateModel() { Abbreviation = "TX", Name = "Texas" });
            states.Add(new StateModel() { Abbreviation = "UT", Name = "Utah" });
            states.Add(new StateModel() { Abbreviation = "VT", Name = "Vermont" });
            states.Add(new StateModel() { Abbreviation = "VA", Name = "Virginia" });
            states.Add(new StateModel() { Abbreviation = "WA", Name = "Washington" });
            states.Add(new StateModel() { Abbreviation = "WV", Name = "West Virginia" });
            states.Add(new StateModel() { Abbreviation = "WI", Name = "Wisconsin" });
            states.Add(new StateModel() { Abbreviation = "WY", Name = "Wyoming" });
            return states.ToList();
        }
    }
}