import { useCallback, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import { Button, ThemeProvider, createTheme } from '@mui/material'

function App() {
    const [count, setCount] = useState(0)
    const getCssVariable = useCallback((name: string) => getComputedStyle(document.body).getPropertyValue(name).trim(), [])
    const theme = createTheme({
        palette: {
            primary: {
                main: getCssVariable("--primary")
            },
            secondary: {
                main: getCssVariable("--secondary")
            },
            warning: {
                main: getCssVariable("--warning")
            },
            error: {
                main: getCssVariable("--error")
            },
            success: {
                main: getCssVariable("--success")
            },
            info: {
                main: getCssVariable("--info")
            }
        }
    })

    return (
        <ThemeProvider theme={theme}>
            <div className="App">
                <div>
                    <a href="https://vitejs.dev" target="_blank">
                        <img src={viteLogo} className="logo" alt="Vite logo" />
                    </a>
                    <a href="https://reactjs.org" target="_blank">
                        <img src={reactLogo} className="logo react" alt="React logo" />
                    </a>
                </div>
                <h1>Vite + React</h1>
                <div className="card">
                    <Button variant="contained" color="success" className="rounded-full"
                        onClick={() => setCount((count) => count + 1)}>
                        count is {count}
                    </Button>
                    <p>
                        Edit <code>src/App.tsx</code> and save to test HMR
                    </p>
                </div>
                <p className="read-the-docs">
                    Click on the Vite and React logos to learn more
                </p>
            </div>
        </ThemeProvider>
    )
}

export default App