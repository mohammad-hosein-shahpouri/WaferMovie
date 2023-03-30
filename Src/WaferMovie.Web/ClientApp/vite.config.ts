import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import { readdirSync } from 'fs'
import { join, resolve } from 'path';

const absolutePathAliases: { [key: string]: string } = {};
const srcPath = resolve('./src');
const srcRootContent = readdirSync(srcPath, { withFileTypes: true }).map((dirent) => dirent.name.replace(/(\.ts){1}(x?)/, ''));

srcRootContent.forEach((directory) => {
    absolutePathAliases[directory] = join(srcPath, directory);
});

// https://vitejs.dev/config/
export default defineConfig({
    resolve: {
        alias: {
            ...absolutePathAliases
        }
    },
    server: {
        port: Number(process.env.PORT) || 5173,
        hmr: {
            protocol: 'ws',
            host: 'localhost'
        },
        host: "0.0.0.0"
    },
    plugins: [react()],
})