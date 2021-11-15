const express = require('express');
const app = express();


const hostname = '127.0.0.1';
const port = 3000;

app.use(express.static('./'));
app.use(express.static('../05_TestBuilds/test'));

app.get('/', (req, res) =>{
	res.send('Hello World!')
});




app.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
