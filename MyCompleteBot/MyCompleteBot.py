from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import httpx
import os

class ChatRequest(BaseModel):
    prompt: str
    model: str = "llama2"
    max_tokens: int = 300

app = FastAPI(title="Ollama FastAPI Chatbot")

@app.post("/chat")
async def chat(req: ChatRequest):
    """Proxy endpoint that forwards prompts to a local Ollama server.

    Environment:
      OLLAMA_URL (optional) - full URL to Ollama generate endpoint (default http://localhost:11434/api/generate)
    """
    ollama_url = os.getenv("OLLAMA_URL", "http://localhost:11434/api/generate")

    payload = {
        "model": req.model,
        "prompt": f"System: You are a helpful assistant.\nUser: {req.prompt}\nAssistant:",
        "max_tokens": req.max_tokens,
    }

    async with httpx.AsyncClient(timeout=60) as client:
        try:
            resp = await client.post(ollama_url, json=payload)
        except httpx.RequestError as e:
            raise HTTPException(status_code=500, detail=f"Request error: {e}")

    if resp.status_code != 200:
        raise HTTPException(status_code=resp.status_code, detail=resp.text)

    return resp.json()

if __name__ == "__main__":
    import uvicorn
    uvicorn.run("MyCompleteBot:app", host="0.0.0.0", port=8000, reload=True)
