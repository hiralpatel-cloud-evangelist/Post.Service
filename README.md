# Post.Service

Welcome to the Post.Service Microservice repository! This repository contains the code for managing blog posts.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## Introduction

The Post.Service project is designed to provide functionality for managing blog posts using the CQRS (Command Query Responsibility Segregation) and Mediator design patterns. It includes features for creating, updating, retrieving, and deleting blog posts. The project is built using .Net CORE, leveraging the power of CQRS and Mediator for better separation of concerns and efficient handling of commands and queries.


## Features

- Create new blog posts with image.
- Update existing blog post details.
- Retrieve blog posts based on various search criteria.
- Delete blog posts by their unique identifiers.


## Getting Started

To get started with the Post.Service project, follow these steps:

1. [Installation instructions](#installation-instructions)
2. [Configuration setup](#configuration-setup)
3. [Running the project](#running-the-project)

### Installation Instructions

1. Clone the repository: `git clone https://github.com/hiralpatel-cloud-evangelist/Post.Service.git`
2. Navigate to the project directory: `cd Post.Service`
3. Install dependencies: `dotnet restore`

### Configuration Setup

To configure the Post.Service project:

1. Rename `appsettings.json.example` to `appsettings.json`.
2. Open `appsettings.json` and update the configuration settings.

### Running the Project

To run the Post.Service project:

1. Build the project: `dotnet build`
2. Run the application: `dotnet run`
3. Access the application in a web browser at `http://localhost:5000`

## Usage

Once the application is up and running, you can use the provided API endpoints to manage blog posts. Detailed API documentation can be found at [http://52.186.89.164/swagger/index.html](http://52.186.89.164/swagger/index.html).

## Architecture

![image](https://github.com/hiralpatel-cloud-evangelist/Post.Service/assets/133631869/73efe946-996e-44a8-9456-c862a4df40c7)



## API Documentation

Detailed API documentation can be found at [http://52.186.89.164/swagger/index.html](http://52.186.89.164/swagger/index.html).

Postman collection : https://documenter.getpostman.com/view/25643962/2s9Y5Zw2e2#ea2ac729-2186-4d79-a818-e384f8473a82  

## Contributing

Contributions to the Post.Service project are welcome! If you find any issues or have improvements to suggest, please follow the steps in [CONTRIBUTING.md](./CONTRIBUTING.md) to contribute.

**Note:** Replace placeholders in square brackets with actual content relevant to your project.

For more detailed information, consider providing sections like "Deployment," "Testing," "Dependencies," and any other relevant details specific to your project.
