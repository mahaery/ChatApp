# Chat Application
This project is a support chat system that allows users to initiate support requests and have their chats assigned to available agents based on team capacity and seniority. The system manages chat sessions through a FIFO queue, monitors session activity, and handles overflow scenarios during peak times.

## Basic Scenario and Usage
Before everything make sure you have docker installed and running on your machine, then run the docker file
```
docker-compose up
```
To use the support chat system:

1. Initiate a support request through the swagger UI by calling the relative endpoint.
2. The chat session is created and added to the queue.
3. The Consumer application will run in the background and gets the chats arrived in queue (FIFO).
4. If there are any available agenst, the chat is assigned to them based on capacity and seniority.
5. Chats are assigned to agents in a round-robin fashion, considering seniority and team capacity.
6. Then the chat will get removed from the session queue once it successfully assigned to an Agent.
