import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig(({ mode }) => {
  return {
    plugins: [react()],
    define: {
      __DEV__: mode === 'development',
    },
    base: '/',
    build: {
      outDir: 'dist',
      sourcemap: mode === 'production' ? false : true,
      terserOptions: {
        compress: {
          drop_console: mode === 'production' ? false : undefined,
          pure_funcs: mode === 'production' ? ['console.warn'] : [],
        },
      },
    },
    server: {
      port: 3000,
      open: true,
    },
  };
});
