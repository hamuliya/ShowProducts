//setupProxy has some problem need be fixed.


const { createProxyMiddleware } = require('http-proxy-middleware');
const {env}=require("process");

const target = env.ASPNETCORE_HTTPS_PORT ? `http://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
   env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7170';

const context=[
  "/Photo",
  "/Product",
]


module.exports = function(app) {
     const appProxy=createProxyMiddleware(context,{
        target: target,
        secure:false
      });
     app.use(appProxy);
    }
  