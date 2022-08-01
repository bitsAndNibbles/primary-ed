#!/bin/env python3

from http.server import BaseHTTPRequestHandler, HTTPServer
import logging
import sys

from prime import next_prime

logging.basicConfig(
    level=logging.DEBUG,
    stream = sys.stdout,
    format='%(asctime)s %(message)s')
LOGGER=logging.getLogger("prime-server")

class RequestHandler(BaseHTTPRequestHandler):

    def do_GET(self):

        # expect the path to be "/n" where n is an integer input
        n = int(self.path[1:])

        self.send_response(200)
        self.send_header("Content-type", "text/plain")
        self.end_headers()

        LOGGER.info(f"finding next prime greater than {n}")

        prime = next_prime(n)

        LOGGER.info(f"{prime} follows {n}")

        # write our response to the output stream
        self.wfile.write(bytes(f"{prime}", "utf-8"))


if __name__ == '__main__':

    address = "0.0.0.0"
    port = 31147

    server = HTTPServer((address, port), RequestHandler)

    LOGGER.info(f"listening on {address}:{port}")

    try:
        server.serve_forever()
    except KeyboardInterrupt:
        pass

    server.server_close()


# to test this server, find the next prime after 1009:
# curl -G http://localhost:31147/1009
