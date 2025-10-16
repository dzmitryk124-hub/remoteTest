import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import FileUpload from './FileUpload.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <FileUpload />
    <App />
  </StrictMode>,
)
