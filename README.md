# Caller Directory API (CDR)

***This my solution to the technical test from Giacom and Union Street.***

## Technology Decisions

- ASP.NET Core (.NET 7)
  - Whilst an API can be written in almost any language, ASP.NET provides a very solid foundation and a very popular ecosystem, with relatively fast prototyping (like a few interpreted languages but with better performance).
- Entity Framework
  -  Overall is a solid ORM that provides all the basic functionality I need for this API (and way more).
- MySQL
  - Whilst Entity Framework can work with various database providers, MySQL is both fast and simple to get started with. MySQL is also incredibly popular, battle-tested, and production-ready. 
- NGINX
  - Whilst there really is numerous ways in deploying this API, I prefer NGINX to act as a proxy to the API and handle the web requests. It strikes a good balance between not being overly-complex but also offering good performance. NGINX is also wildly popular and battle-tested. 

## Architecture Decisions

- Using ASP.NET Core I've created a "CallsController". This controller contains the endpoints that the API provides.
- The "Services/DataUploadService" contains the logic that parses the file that's been uploaded and enqueues it.
- The "Worker/DataUploadQueue" provides a persistent queue that can be shared between the service and the background service. It has an interface that provides the most basic but essential functionality.
- The "Worker/DataUploadWorker" is a service that runs in the background on a separate thread. The sole purpose of this service is to continuously process the models that have been imported but haven't been persisted to the database.
  - This approach has allowed me to handle the slow database updates in a separate thread, so it's not blocking/slowing down web requests from the API.  
 
## Assumptions:

- I've assumed the data has come from a source that ensures the output is correct. I've done no verification such as ensuring the columns are in the right order, or any data integrity checks.
- The cost is always in GBP. There is no currency conversion when running calculations on this column.
- There's no file size restrictions. This obviously isn't ideal, but since the only description is the file could be at least a few GB, I've removed the limit. This should be revisited if this was used in production.
- I've assumed phone numbers are always the same format (only numbers).

## Given More Time

- I'd have added checks to ensure data integrity when importing.
- When uploading a file, the server should save it to a temporary path and then queue the file to be parsed via a separate program. Currently, if a large file is uploaded, it'll parse the file on the thread that responds to the web request (causing it to delay the response).
- Support uploading files using streaming. This would allow web requests to upload large files in chunks instead of all at once.
- The API should really be versioned. As this is a very basic API, I've decided to put this on the backlog for when I would've had more time.
- Currently, the background service worker runs in what is essentially a while true loop. I'd be much happier if there was some blocking code to free up the CPU a bit more when the queue is empty (I'm thinking something along the lines of AutoResetEvent).
- Since this data is updated at predictable intervals (probably daily according to the brief) it's a perfect example of data that should be cached using something such as Redis.
- Add unit and feature tests for each endpoint and service. The code is structured with testability in mind, so adding them will not require code-changes to codebase.

## Endpoints

### List records

Get a list of all records:

    - [GET]: /

### Getting a record

Get a record using its reference key:

    - [GET]: /{reference}

### Get caller records

Get a list of records made by a specific caller:

    - [GET]: /caller/{callerId}

### Get unknown caller records

Get a list of all records made by an unknown caller:

    - [GET]: /caller/unknown

### Get recipient records

Get a list of all records for a specific recipient:

    - [GET]: /recipient/{recipientId}

### Get all costs for a caller

Get the min and max costs for each caller:

    - [GET]: /costs

### Get an hourly breakdown of costs

Get the max and average costs for each hour of a day (24-hour window):

    - [GET]: /costs/hourly

### Import data

Import data by uploading a CSV file:

    - [POST]: /import

## Filtering results

You can filter results by providing a `StartDateTime` and `EndDateTime`:


## Sorting results

You can sort results by providing a `SortColumn` and a `SortDirection`. Currently, only one column can be sorted at a time.

## Pagination

You can paginate data by providing `Page` and `PerPage`.

### Notes

- `Cost` is returned as an integer. To get the decipence value of the cost, make sure you **divide by 1000**.