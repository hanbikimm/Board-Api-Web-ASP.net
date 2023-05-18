module.exports = {
  devServer: {
    proxy: { 
      '^/Notice': {
        target: 'http://bootcamp.com/', 
        changeOrigin: true,
        logLevel: 'debug',
      },

      '^/Comment': {
        target: 'http://bootcamp.com/', 
        changeOrigin: true,
        logLevel: 'debug',
      },
      
      '^/Member': {
        target: 'http://bootcamp.com/', 
        changeOrigin: true,
        logLevel: 'debug',
      }

      
    },
  },
};