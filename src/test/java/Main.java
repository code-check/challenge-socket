import org.junit.Test;

import java.io.DataOutputStream;
import java.net.ServerSocket;
import java.net.Socket;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNotNull;

public class Main {
    private Client client;
    private ServerSocket server;
    public Socket caughtClient;


    @Test
    public void tests() {
        // Step 1
        try {
            server = new ServerSocket(0);
            Test1 test = new Test1(server, this);
            test.start();
            await();

            // Prevent blocking calls to escalate
            Thread tClient = new Thread(new Runnable() {
                public void run(){
                    client = new Client();
                    client.connect("127.0.0.1", server.getLocalPort());
                }
            });
            tClient.start();

            await();
            assertNotNull(caughtClient);
        } catch (Exception ex) {
            assertFalse("Unknown error thrown in Step 1", true);
        }

        // Step 2
        try {
            Test2 test = new Test2(server, caughtClient);
            test.start();
            await();
            DataOutputStream out = new DataOutputStream(caughtClient.getOutputStream());
            out.writeBytes("Pong\n");
            await();
            assertNotNull("No message received", test.message);
            assertEquals("Unexpected message", "Hello World!", test.message);
        } catch (Exception ex) {
            assertFalse("Unknown error thrown in Step 2", true);
        }

        // Step 3
        try {
            Test3 test = new Test3(server, caughtClient);
            test.start();
            await();
            DataOutputStream out = new DataOutputStream(caughtClient.getOutputStream());
            out.writeBytes("Marco\n");
            await();
            assertNotNull("No message received", test.message);
            assertEquals("Unexpected message", "Polo", test.message);
        } catch (Exception ex) {
            assertFalse("Unknown error thrown in Step 3", true);
        }
    }

    private void await() throws InterruptedException {
        synchronized (server) {
            server.wait(1000);
        }
    }
}
