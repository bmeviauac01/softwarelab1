# Exercise 3: Task operations

Implement two new endpoints in the controller handling tasks that alter existing tasks as follows.

**You can earn 3-3 points with the completion of these exercises.**

### Marking a task as done

The flag `Task.Done` signals that a task is completed. Create a new http endpoint that uses the `ITasksRepository.MarkDone` method to set this flag on a task instance.

Request: `PATCH /api/tasks/neptun/{id}/done` with `{id}` being the tasks ID.

Response:

- `404 Not found` if no such task exists.
- `200 OK` if the operation was successful - returns the task in the body after the modification is done.

### Move to new status

A task is associated with a status through `Task.StatusId` (or similar). Create a new http endpoint that uses the `ITasksRepository.MoveToStatus` method to change the status of the specified tasks to a new one. If the new status with the provided name does not exist, create one.

Request: `PATCH /api/tasks/neptun/{id}/move?newStatusName=newname` with

- `{id}` is the tasks ID,
- and the **name** of the new status is received in the `newStatusName` query parameter.

Response:

- `404 Not found` if no such task exists.
- `200 OK` if the operation was successful - returns the task in the body after the modification is done.

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response from among the two above. Save the screenshot as `f3.png` and submit with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.
