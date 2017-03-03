# QueryAsJson

## Current Version 0.8.0

for dotnet core 1.0, 1.1 and .NET Framework 4.0 and higher


## Nuget
[Nuget Package](https://www.nuget.org/packages/Newtonsoft.Json/10.0.1-beta1)
```
PM> Install-Package QueryAsJson.Core
```


## Description
This library helps to format query results (from a database) as JSON with arbitrary structure.

Some database engines lack the ability to query tables an return the results directly as JSON. This can be accomplished with QueryAsJson by an intuitve API based on anonymous types. 

Basicly this library was created to test some features of dotnet core.

## Usage

```csharp
Define.QueryWithNestedResults("cid",
@"select c.id as cid, c.sname, c.fname, c.BDAY, c.ADR_STREET, c.ADR_CITY, i.id as iid, i.inv_date, i.amount, o.articles, o.id as oid
from customer c
left join orders o on c.id = o.customer_id
left join invoices i on c.id = i.customer_id"
,
    new
    {
        Id = Define.Column("cid"),
        SurName = Define.Column("sname"),
        FirstName = Define.Column("fname"),
        Birthday = Define.Column("BDAY"),
        Address = new
        {
            Street = Define.Column("ADR_STREET"),
            City = Define.Column("ADR_CITY"),
        },
        Invoices = Define.NestedResults("iid",
            new
            {
                InvoiceId = Define.Column("iid"),
                Inv_Date = Define.Column(),
                TotalAmount = Define.Column("amount"),
            }
        ),
        Orders = Define.NestedResults("oid",
            new
            {
                OrderId = Define.Column("oid"),
                ArticleSummary = Define.Column("articles"),
            }
        )
    }
)
```

Creates something like this

```json
[{
    "Id": 1,
    "SName": "Meyers",
    "FirstName": "Mike",
    "LastUpdate": "2017-03-01",
    "Birthday": "1983-10-31 00:00:00.000",
    "Address": {
        "Street": "Street 1",
        "City": "Las Vegas"
    },
    "Invoices": [{
            "InvoiceId": 1,
            "Inv_Date": "2015-10-12 15:43:00.000",
            "TotalAmount": 500.2
        }, {
            "InvoiceId": 2,
            "Inv_Date": "2016-05-09 15:43:00.000",
            "TotalAmount": 155.2
        }
    ],
    "Orders": [{
            "OrderId": 3,
            "ArticleSummary": "Book 1, Book 2"
        }, {
            "OrderId": 2,
            "ArticleSummary": "CD Burner, Usb stick 32Gb"
        }
    ]
}]
```