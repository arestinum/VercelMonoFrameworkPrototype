#!/bin/bash
echo "Starting XSP4 server..."
ls -la /app
xsp4 --port 8080 --address 0.0.0.0 --root /app --nonstop --verbose