## Simple Like Systme

## Questions

### How can you improve the feature to make it more resilient against abuse/exploitation?

By Adding a Authentication and Authorization with email otp verification to ensure one like per authorized person. This will mitigate abuse and exploitation. Requiring users to verify their accounts will limit the use of fake accounts.

### How can you improve the feature to make it scale to millions of users and perform without issues?

By using a lock mechanism to ensure that only one user can increment the like count at a time. This will ensure that the feature can perform without issues. Also a queue system could be a good solution as it can be used to to offload the like increment operation to background workers

### How will you scale to a million concurrent users clicking the button at the same time

By adding a queueing system to reduce load on the server for increment of like count. Also Batch processing could be implemented to reduce the number of database calls.

### How will you scale to a million concurrent users requesting the article's like count at the same time

By using a cache db like Redis to store the like counts and and can easily be fetched concurrently by users

Blog Guide: https://medium.com/@KumarHalder/token-based-authentication-in-asp-net-core-43e99aee0593
