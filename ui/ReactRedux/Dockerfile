# Use an official node image as a parent image
FROM node:18-alpine as build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code to the working directory
COPY . .

# Build the application (with the production environment by default)
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL
RUN npm run build

# Use nginx as a web server to serve the production build
FROM nginx:alpine

# Copy the nginx configuration file
COPY ./nginx.conf /etc/nginx/nginx.conf

# Copy the build files to the nginx html folder
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the port
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
