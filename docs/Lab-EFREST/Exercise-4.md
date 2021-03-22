# Exercise 4: Optional exercise

**You can earn an additional +3 points with the completion of this exercise.** (In the evaluation, you will see the text "imsc" in the exercise title; this is meant for the Hungarian students. Please ignore that.)

If we have lots of tasks listing them should not return all of them at once. Implement a new endpoint to return a subset of the tasks (i.e., "paging"):

- It returns the tasks in a deterministic fashion sorted by ID.
- The query accepts a `count` parameter that specifies how many tasks to return.
- Specifying the next page is performed via a `from` parameter. This `from` is the ID of the first item to return on this page.
- Both the `from` and `count` are specified as query parameters.
- The new paging endpoint should be available on URL `GET /api/tasks/neptun/paged` (the `/paged` part is necessary so that the previous listing endpoint also remains functional).
- The response should return an instance of class `Controllers.Dto.PagedTaskList`. This includes:
    - `items`: an array containing the tasks on the current page,
    - `count`: specifying the number of items on the current page,
    - `nextId`: id of the next task - to fetch the next page (to be used in `from`),
    - `nextUrl`: a helper URL that fetches the next page, or `null` if there are no further pages.

        !!! tip ""
            Use the `Url.Action` helper method to assemble this URL. Do not hardcode "localhost:5000" or "/api/tasks/paged" in the source code! You will _not_ need string operations to achieve this.

            `Url.Action` will give you an absolute URL when all parameters (`action`, `controller`, `values`, `protocol`, and `host`) are specified; for the latter ones `this.HttpContext.Request` can provide you the required values.

- The request always returns 200 OK; if there are no items, the result set shall be empty.

The requests-responses shows you the expected behavior:

1. `GET /api/tasks/neptun/paged?count=2`

    This is the first request. There is no `from` value specified to start from the first item.

    Response:

    ```json
    {
      "items": [
        {
          "id": 1,
          "title": "doing homework",
          "done": false,
          "status": "pending"
        },
        {
          ID: 2,
          "title": "doing more homework",
          "done": false,
          "status": "new"
        }
      ],
      "count": 2,
      "nextId": 3,
      "nextUrl": "http://localhost:5000/api/tasks/neptun/paged?from=3&count=2"
    }
    ```

2. `GET /api/tasks/neptun/paged?from=3&count=2`

    This is to query the second page.

    Response:

    ```json
    {
      "items": [
        {
          "id": 3,
          "title": "hosework",
          "done": true,
          "status": "done"
        }
      ],
      "count": 1,
      "nextId": null,
      "nextUrl": null
    }
    ```

    The response indicates no further pages as both `nextId` and `nextUrl` are null.

3. `GET /api/tasks/neptun/paged?from=999&count=999`

    Returns an empty page.

    Response:

    ```json
    {
      "items": [],
      "count": 0,
      "nextId": null,
      "nextUrl": null
    }
    ```

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response of fetching a page. Save the screenshot as `f4.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.
