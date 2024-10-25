
# Project Overview

This project is a robust front-end application that leverages **MobX** for state management alongside several modern web development technologies. Below is a detailed overview of its features, structure, and key components that demonstrate its power, flexibility, and scalability.

## Key Features:
1. **Localization**: Built-in support for multiple languages using `react-i18next`, enabling global accessibility with dynamic language switching.
2. **Routing**: Powered by `react-router-dom`, it offers a seamless navigation experience across different pages and protected routes.
3. **Error Boundary**: Implements custom error boundaries to gracefully handle unexpected errors, ensuring users do not encounter crashes.
4. **Design & Styling**: Uses `Ant Design` for a comprehensive UI library and `Tailwind CSS` to maintain a consistent and scalable design system.
5. **Authentication**: Fully integrated JWT-based authentication system (`axios-jwt`), including login, logout, and protected routes for role-based access (e.g., super admin and regular users).
6. **Task Management**: Users can add, edit, and delete tasks with real-time updates. The UI reflects these changes with `Ant Design` components such as tables, modals, and notifications.
7. **User Management**: Super admin users have the ability to manage other users, including adding, removing, and editing user details.
8. **Role-based Access**: Comprehensive role management, ensuring only authorized users can access specific functionalities.
9. **Security**: Uses secure local storage for tokens with `secure-ls`, ensuring data safety.
10. **Email Verification and Password Management**: Users can change passwords, reset forgotten passwords, and verify their email addresses.

## Technologies Used

- **MobX**: A simple and scalable state management library that provides fine-grained reactivity. MobX helps manage complex state requirements in the project.
- **React**: Used to build the UI components and handle client-side logic.
- **Ant Design**: Provides a powerful and customizable set of UI components.
- **TailwindCSS**: Enables responsive and utility-first CSS design, which is customizable and easy to use.
- **DayJS**: A fast and small library for parsing, validating, and formatting dates.
- **JWT Authentication**: Secure and stateless authentication mechanism using JSON Web Tokens (JWT).
- **Axios**: Simplifies HTTP requests to communicate with the backend services.
- **React Router DOM**: Handles navigation and dynamic routing within the app.
- **Docker**: Used to containerize the front-end application and serve it using **Nginx**. The containerized app can be easily deployed and scaled, providing a consistent environment across different platforms.

## Project Structure

```bash
.
├── .dockerignore
├── .env
├── .gitignore
├── Dockerfile
├── README.md
├── package.json
├── tsconfig.json
├── vite.config.ts
├── src
│   ├── assets
│   ├── auth
│   ├── components
│   ├── error-handlers
│   ├── localization
│   ├── models
│   ├── modules
│   ├── pages
│   ├── routing
│   ├── services
│   ├── stores
│   ├── theme-configs
│   └── tests
└── public
```
This structure ensures the separation of concerns, making it easier to scale and maintain the application.

## Key Directories

- **`src/auth`**: Contains the authentication-related forms, services, and utilities.
- **`src/components`**: Shared components like layout helpers, navigation menus, and notification handlers.
- **`src/localization`**: Configuration for `react-i18next` that powers multi-language support.
- **`src/stores`**: MobX stores responsible for managing state across various parts of the application.
- **`src/modules`**: Different application modules such as user profile management, task management, and user management.
- **`src/error-handlers`**: Contains error boundary components for graceful error handling.
- **`src/routing`**: Handles routing configurations for the application.

## Package Dependencies

```json
{
  "dependencies": {
    "@ant-design/icons": "^5.4.0",
    "antd": "^5.20.6",
    "antd-style": "^3.6.3",
    "aos": "^2.3.4",
    "axios": "^1.7.7",
    "axios-jwt": "^4.0.3",
    "bootstrap": "^5.3.3",
    "bootstrap-icons": "^1.11.3",
    "dayjs": "^1.11.13",
    "jwt-decode": "^4.0.0",
    "mobx-react-lite": "^4.0.7",
    "react": "^18.3.1",
    "react-dom": "^18.3.1",
    "react-i18next": "^15.0.2",
    "react-router-dom": "^6.26.2",
    "secure-ls": "^2.0.0",
    "swiper": "^11.1.14"
  },
  "devDependencies": {
    "@eslint/js": "^9.9.0",
    "@types/aos": "^3.0.7",
    "@types/react": "^18.3.3",
    "@types/react-dom": "^18.3.0",
    "@vitejs/plugin-react": "^4.3.1",
    "eslint": "^9.9.0",
    "eslint-plugin-react-hooks": "^5.1.0-rc.0",
    "eslint-plugin-react-refresh": "^0.4.9",
    "globals": "^15.9.0",
    "typescript": "^5.5.3",
    "typescript-eslint": "^8.0.1",
    "vite": "^5.4.1"
  }
}
```
# Environment Variables Setup

To configure the API URL for the backend, follow these steps:

1. In the root of your project directory, create a new file called `.env.local` (if it doesn't already exist).
2. Open the `.env.local` file in your code editor.
3. Add the following line to specify the backend URL: 

```bash
VITE_API_URL=https://localhost:7299/api
```


This will ensure that your frontend project points to the correct backend API when running locally.


## How to Run

1. Install dependencies:
    ```bash
    npm install
    ```
2. Start the development server:
    ```bash
    npm run start:dev
    ```
3. Build the project:
    ```bash
    npm run build
    ```


# Dockerization

This project is Dockerized for ease of deployment and development. It uses a multi-stage build to optimize the final image size and performance:

1. **Node.js Build Stage**:
   - We use the official Node.js 18-alpine image as a base for the build.
   - The Node.js build process is used to install dependencies and build the front-end assets, which are then used in the production environment.

2. **Nginx Web Server**:
   - Once the assets are built, we use the official Nginx Alpine image to serve the production build.
   - The `nginx.conf` file is used to configure Nginx to serve the static files and proxy API requests to the backend.

3. **Nginx Configuration**:
   - The `nginx.conf` ensures that:
     - All static files are served from the `/usr/share/nginx/html` directory.
     - The front-end routing works by redirecting all requests to `index.html`.
     - API requests starting with `/api/` can be proxied to a backend server.

   ```nginx
   server {
       listen 80;
       server_name localhost;
       root /usr/share/nginx/html;
       index index.html index.htm;

       location / {
           try_files $uri $uri/ /index.html;
       }

       location /api/ {
           proxy_set_header Host $host;
           proxy_set_header X-Real-IP $remote_addr;
           proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
           proxy_set_header X-Forwarded-Proto $scheme;
       }
   }
   ```

4. **Environment Variable Support**:
   - The `VITE_API_URL` environment variable can be passed into the build to dynamically set the backend API URL.

5. **Running the Container**:
   - Build the Docker image:
     ```bash
     docker build -t my-app .
     ```
   - Run the container:
     ```bash
     docker run -p 8080:80 my-app
     ```

The project is now set up to be run in a lightweight container with Nginx serving the static assets and routing API calls to the backend.


## Highlights:
This project demonstrates a deep understanding of modern frontend technologies, featuring:
- **Scalability**: Through Mobox state management and modular code architecture.
- **User Experience**: By providing dynamic interfaces with real-time updates using Ant Design and Tailwind CSS.
- **Security & Authentication**: JWT-based authentication and role-based access, ensuring secure access to sensitive functionalities.
- **Localization**: Fully supports multilingual interfaces, making the product globally accessible.

This frontend is highly customizable and demonstrates proficiency in building secure, scalable, and visually appealing applications.