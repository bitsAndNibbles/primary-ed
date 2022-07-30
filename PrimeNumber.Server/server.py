#!/bin/env python3

from http.server import BaseHTTPRequestHandler, HTTPServer

from prime import next_prime

class RequestHandler(BaseHTTPRequestHandler):

    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-type", "text/plain")
        self.end_headers()

        # expect the path to be "/n" where n is an integer input
        n = int(self.path[1:])

        prime = next_prime(n)

        # write our response to the output stream
        self.wfile.write(bytes(f"{prime}", "utf-8"))


if __name__ == '__main__':

    address = "0.0.0.0"
    port = 31147

    server = HTTPServer((address, port), RequestHandler)

    print(f"listening on {address}:{port}")

    try:
        server.serve_forever()
    except KeyboardInterrupt:
        pass

    server.server_close()


# to test this server, find the next prime after 1009:
# curl -G http://localhost:31147/1009
