# Log

## Asummptions
1. IoC container would be setup in each of the products project files or entry points. 
2. Is Kyc service returns a IsSuccess=false can assume that Kyc check is a fail but should fail nicely.
3. A single application is for a specific product. have assumed validation that checks the correct productType on application
### Initial Assumptions
1. Since Services.AdministratorOne.Abstractions is a third party library, then so must be Services.AdministratorTwo.Abstractions. Because of this I will not touch these projects but try to use the interfaces provided in respective products...


## Decisions
1. Testing - Probably out of scope given time and challenge. Plenty of logic to verify so lends itself to simple unit tests.
2. Seperate projects for each product to illustrate two potential completely seperate code bases or repo's
3. Due to time, decided against abstracting the validation age range and payment values into a config file. Hardcoded in product processors for now 
## Observations
1. Initial payment on the Application object is a decimal. This implies that users can make payments in decimal however AdministratorOne only accepts integer values. Would need to either assume some rounding here OR validate that the input can be parsed to a int.

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
2. Abstract the call to AdministrationService a service that returns a common type so that it can easily be swapped out if required

