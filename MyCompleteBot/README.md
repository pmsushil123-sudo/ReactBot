FastAPI + Ollama chatbot

Run locally:

1. Install dependencies:
   python -m pip install -r requirements.txt

2. Ensure Ollama is running locally (example):
   ollama serve

3. Start the FastAPI app:
   python MyCompleteBot.py

4. Send a chat request:
   curl -X POST "http://localhost:8000/chat" -H "Content-Type: application/json" -d '{"prompt":"Hello, how are you?","model":"llama2","max_tokens":150}'

Environment:
- OLLAMA_URL: optional. Default is http://localhost:11434/api/generate

Notes:
- This app proxies requests to the Ollama /api/generate endpoint. Adjust payload fields if your Ollama version uses a different API shape.
