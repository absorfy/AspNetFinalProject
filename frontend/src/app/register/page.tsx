"use client";
import { useState } from "react";

export default function LoginPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const res = await fetch("http://localhost:5134/api/auth/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ email, password, rememberMe: true }),
        });

        if (res.ok) alert("Logged in!");
        else alert("Error");
    };

    return (
        <form onSubmit={handleSubmit}>
            <input type="email" value={email} onChange={e=>setEmail(e.target.value)} placeholder="Email"/>
            <input type="password" value={password} onChange={e=>setPassword(e.target.value)} placeholder="Password"/>
            <button type="submit">Login</button>
        </form>
    );
}
