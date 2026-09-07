import { useState } from 'react';
import './App.css';

function App() {
    const [message, setMessage] = useState('');
    type ChatResp = {
        model?: string;
        createdAt?: string;
        response?: string;
        raw?: string;
    };

    const [reply, setReply] = useState<ChatResp | null>(null);
    const [loading, setLoading] = useState(false);

    async function send() {
        if (!message) return;
        setLoading(true);
        setReply(null);
            try {
            const resp = await fetch('/api/chat', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ message })
            });
            if (!resp.ok) {
                    setReply({ response: `Error: ${resp.status}` });
            } else {
                    const json = await resp.json();
                    // Normalize possible field names (created_at vs createdAt)
                    const model = json.model ?? json.Model;
                    const createdAt = json.createdAt ?? json.created_at ?? json.CreatedAt;
                    const responseText = json.response ?? json.Response ?? json.reply ?? json.Raw ?? json.raw ?? JSON.stringify(json);
                    setReply({ model, createdAt, response: responseText, raw: JSON.stringify(json, null, 2) });
            }
        } catch (err) {
                setReply({ response: String(err) });
        } finally {
            setLoading(false);
        }
    }

    return (
        <div style={{ padding: 20 }}>
            <h1>ReactBot Chat</h1>
            <p>Type a message and press Send to chat with the Ollama-backed bot.</p>
            <div style={{ marginBottom: 8 }}>
                <input value={message} onChange={e => setMessage(e.target.value)} style={{ width: '60%' }} placeholder="Hello" />
                <button onClick={send} disabled={loading} style={{ marginLeft: 8 }}>Send</button>
            </div>
            <div>
                {loading && <div>Loading...</div>}
                {reply && (
                    <div style={{ background: '#f3f3f3', padding: 10 }}>
                        {reply.model && <div style={{ fontSize: 12, color: '#666' }}>Model: {reply.model}</div>}
                        {reply.createdAt && <div style={{ fontSize: 12, color: '#666' }}>Created: {reply.createdAt}</div>}
                        <div style={{ whiteSpace: 'pre-wrap', marginTop: 8 }}>{reply.response}</div>
                        {reply.raw && (
                            <details style={{ marginTop: 8 }}>
                                <summary>Raw JSON</summary>
                                <pre style={{ background: '#fff', padding: 8, overflow: 'auto' }}>{reply.raw}</pre>
                            </details>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}

export default App;
