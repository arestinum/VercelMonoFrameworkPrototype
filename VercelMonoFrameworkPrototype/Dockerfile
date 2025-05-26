FROM mono:latest

# Install XSP (Mono's lightweight web server)
RUN apt-get update && apt-get install -y mono-xsp4

# Create app directory
WORKDIR /app

# Copy your application files
COPY . .

# Create App_Data directory if it doesn't exist
RUN mkdir -p /app/App_Data

# Set permissions
RUN chmod -R 755 /app

# Expose the port XSP will run on
EXPOSE 8080

# Start XSP server
CMD ["/app/start.sh"]