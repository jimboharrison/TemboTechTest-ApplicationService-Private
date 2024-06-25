# Log

## Asummptions
1. IoC container would be setup in each of the products project files or entry points. 
2. Is Kyc service returns a IsSuccess=false can assume that Kyc check is a fail but should fail nicely.
3. A single application is for a specific product. have assumed validation that checks the correct productType on application
### Initial Assumptions
1. Since Services.AdministratorOne.Abstractions is a third party library, it looks like Services.AdministratorTwo.Abstractions is an internal wrapper to an external third party. Will not touch either of these products but it will affect how I implement each product app.
2. Will not change the return type of the IApplicationProcessor.Process method. Ideally for failure we would want to throw an exception and catch or return a nice error to user. Not sure how this product is consumed so will leave for now.


## Decisions
1. Testing - Probably out of scope given time and challenge. Plenty of logic to verify so lends itself to simple unit tests.
2. Seperate projects for each product to illustrate two potential completely seperate code bases or repo's
3. Due to time, decided against abstracting the validation age range and payment values into a config file. Hardcoded in product processors for now
4. Added a common response type for CreateInvestorAndProcessPayment so that external third party types are not required inside product domain code 
## Observations
1. Initial payment on the Application object is a decimal. This implies that users can make payments in decimal however AdministratorOne only accepts integer values. Would need to either assume some rounding here OR validate that the input can be parsed to a int.
2. The 99p minimum payment could and should be extracted into a config file. For now I have gone with the rule of 2/3. Since its only repeated twice, this is okay.

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


## James Notes
1. Both products are abstracted into seperate Console app projects. Looking back, probably not needed to good to demonstrate how these processors may be consumed with a very dummy example of some DI being setup ( not included all services, only examples )
2. AdministratorOne is a third party so have included a wrapper and known types. AdministratorTwo I have assumed is already a wrapper around an external library so have consumed this directly in the product processor code

### If I had more time

1. Unit testing for logic and validation 
2. Abstract out the validation values into a DB or at least config file. Ideally some sort of storage that can be managed outside of product releases.
3. Requirements for exactly what the Products were processing is unclear. It is dealing with money, so assumption is it needs to be secure. If consumed by API would need to handle this. Out of scope for task requirements though.
