# Log

## Asummptions
### Initial Assumptions
1. Since Services.AdministratorOne.Abstractions is a third party library, then so must be Services.AdministratorTwo.Abstractions. Because of this I will not touch these projects but try to use the interfaces provided in respective products...
## Decisions
1. Testing - Probably out of scope given time and challenge. Plenty of logic to verify so lends itself to simple unit tests.
## Observations


## Todo

### Steps requierd for completions ( Requirements / Mental notes )
1. Implement ProductOne 
    1. Using IAdministrationService from AdminOne...Abstractions project.
        1. Single create investor method on interface. 
    2. Implement Validation. Abstract this into some sort of config layer as it looks like a common config accross both products
        1. Age Range
        2. Minimum payment amount
    3. IsVerified check before proceeding to process user
    4. Include domain events at each major step

