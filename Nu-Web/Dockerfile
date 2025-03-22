# Use an official lightweight Node.js image.
FROM node:22-alpine

# Set the working directory inside the container.
WORKDIR /app

# Copy package.json and package-lock.json (if available) from the Nu-Web directory.
COPY Nu-Web/package*.json ./

# Step to fix this Error
# 2025-03-20 21:35:09 nu_web   | Error: Cannot find module @rollup/rollup-linux-x64-musl. 
# npm has a bug related to optional dependencies (https://github.com/npm/cli/issues/4828). Please try `npm i` again after removing both package-lock.json and node_modules directory.
# Remove any existing node_modules and package-lock.json to work around the npm bug.
RUN rm -rf node_modules package-lock.json || true

# Install the dependencies.
RUN npm install

# Copy the rest of your application code.
COPY Nu-Web/ ./

# Expose the port the app runs on (default is 3000 for Vite).
EXPOSE 5173

# Start the Vite development server and bind it to all network interfaces.
CMD ["npm", "run", "dev", "--", "--host"]