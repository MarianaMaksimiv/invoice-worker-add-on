# Invoice Worker

## Introduction 
Invoice Worker add-on Repository. This add-on reads from a json feed and creates PDF files with the invoices detail.

## Architecture Overview

First version:
* Console app tool using local/filesystem storage
    * [Class Diagram](resources/InvoiceWorkerOverview-ClassDiagram.jpg)
    * [Architecture Diagram](resources/InvoiceWorkerOverview-Architecture.jpg)

## Assumptions you made

* Use Anonymous authentication for first version. Connecting to the feed will probably require authentication but let's say that it's not needed for now.
* Don't create invoices if the status is "SENT" or "DELETED"
* Files retention policy:
    * The app will use ALL the storage available. This should be reviewed and adjusted in the future.
    * The app won't run if there is no more storage available
* It won't create invoices for historical records, it means that it will start with the latest invoice (afterEventId=0)
* I don't wanna create a database to store the data from this feed, so I'm assuming for now that INVOICE_UPDATES includes ALL the lines. Therefore I won't be adding logic to merge data, specially given that the updates from the feed are at the Invoice Level and not at the lines, so it would be impossible to rebuild the state for them in some scenarios (i.e if lines were deleted).


## Shortcuts/Compromises made

* I decided to do the first version as a simple console app that stores files in local file system. This doesn't scale well, not to mention that this is not doable in all devices i.e phones, but the code structure is maintanable and easy to extend. The code leverages on interfaces to add more Storage Providers and the core logic is encapsulated in the Business project, which makes the backend code very portable. Also using dotnet core in a simple console app makes the code portable to any cloud provider and different solutions.
* I chose not to create a DB not for lazyness but because I highly believe that keeping track of invoices state from a feed is not a good source of truth, also feels like we are probably duplicating a -better- system/API that could already give us this info.
* I expect that the events data represent a full view of the invoice state at that specific time otherwise my solution won't work.
* System.CommandLine nuget is a package from Microsoft that is on a pre-release version, is not adviced to use pre-releases for prod versions but I still wanted to use it. Why? because it makes me write less code and looks pretty (auto-complete when using the console, help oob, easier to handle/parse commands).
* The library to generate PDFs, iText7, has a limited Open Source version that will work for the tech test (non-commercial is free) but for a prod version it migh be better to dig deeper on a payed version (most of them are not free).


## How you would improve on this design

Get information about how the end user plans to send the invoices to his clients:
* No one should really need a console app that downloads files to the local file system, instead of that we could use a cloud storage and have the logic from the console app running in a function or a Web Job that will scalate better, but that depends on the user requirements in terms of performance.
* Is it possible for the user to actually need invoices on demand vs all of them / live approach?
* The add-on could also automate the task of sending the invoice by email or using a different channel but it depends on the business process around the feature.
* The add-on could be just a simple app that doesn't run the logic and only cares about checking the files in a cloud storage.

Get more information about the Feed component:
* Does it provide a webhook to report changes?
* Does it have PubSub capabilities? also to avoid polling and have a way to be notified in case of a change.
* Can we at least have a HEAD endpoint to check if something changed instead of doing a full GET?

Add more tests:
* Unit tests
* Integration tests
* Component testing
* Smoke Tests for deployment
* Get real data to build better tests

Create more versions with improvements based on what the users need:
 * Support authentication when connecting to the feed.
 * Support more storage providers (Azure Blob Storage, Amazon S3, FTP, etc)
 * Improve delete invoices strategy i.e delete invoices after X amount of time. Get rid of invoices that won't be used anymore to optimize storage space and possible cost specially when using Cloud providers.
 * Improve invoice PDF design to match the company/user standards.
 * Add Application Insights and better telemetry
 
Scale the solution as required. Therefore ask stuff related to performance, i.e number of users, expected traffic:
 * Better support for large noisy feeds (might not be required depending on the nature of the source feed). For example, if the feed starts growing too fast we might want to poll more often or increase the pageSize to make sure that we are not creating files too slowly.
 * Choose a different architecture if scalability is an issue, like Kubernetes, Functions (auto-scaling). This might require having multiple workers and would need a better way to handle some key parameters like "afterEventId" that they'll need to share.
 * Calculate pricing as depending on what you add

Improve Code Base & Tooling:
 * Select a better PDF library
 * Add DI validation on startup
 * Add Code Coverage analysis
 * CI/CD
 * Create a formal business model, right now everything the app is using the Feed model, I prefer not to use classes for data serialization as business models, also to reduce dependency from external systems.
 * Improve exception handling to support more alternative cases.


## Getting Started

Environment Setup (Dev):
* Dotnet Core 3.1
* Visual Studio 2019


## Build and Test

You can download this repo to run/debug the tool:
* Use dotnet command to run it using CLI:
	* Navigate to the src folder: `cd <local_dir_with_the_code>\src`
	* `dotnet run -p ./Fintech.InvoiceWorker.AddOn -- --help`
	* `dotnet run -p ./Fintech.InvoiceWorker.AddOn -- --feed-url=http://www.google.com --invoice-dir=c:\\Users\\Foo`
	> Note: If you don't have a feed url you can just run the test in the Test project from VS, it includes a feed mock.
* Use Visual Studio 2019 to run the solution inside the *src* folder or the Tests.
	> Note: The test will use a default output directory to write the PDFs under *Fintech.InvoiceWorker.UnitTests\bin\Debug\netcoreapp3.1\OutputFiles\*





