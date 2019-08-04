Following are the end points supported by the application. All Post end points 
accept JSON input.

Blogger:
    GET: /api/blogger/{id}
    GET: /api/blogger
    POST: /api/blogger
        sample json: {
                    "id":"103",
                    "firstName":"fff",
                    "lastName":"gfgf"
                    }
    DELETE: /api/{id}


BlogPost:
    GET: /api/post
    GET: /api/post{id}
    POST: /api/post
        sample json: {
                    "bloggerId": "102",
                    "post":{
                        "id":"10060",
                        "subject":"Sample subject",
                        "body":"Sample body"
                    }
                }
    POST: /api/post/bulkAdd
        sample json: [
                        {
                            "bloggerId": "102",
                            "post": {
                                "id": "1011",
                                "subject": "Sample subject",
                                "body": "Sample body"
                            }
                        },
                        {
                            "bloggerId": "102",
                            "post": {
                                "id": "1111",
                                "subject": "Sample subject",
                                "body": "Sample body"
                            }
                        },
                        {
                            "bloggerId": "103",
                            "post": {
                                "id": "1020",
                                "subject": "Sample subject",
                                "body": "Sample body"
                            }
                        }
                    ]
    

Connection:
    POST: /api/connection/create?blogger1={id}&blogger2={id}
    GET: /api/connection/hops?blogger1={id}&blogger2={id}
    GET: /api/connection