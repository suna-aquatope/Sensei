# Redirect server via mitmproxy

## Install mitmproxy

Download the installer from [mitmproxy.org](https://mitmproxy.org/)

## Install CA certificate

Follow the instructions from [System CA on Android Emulator](https://docs.mitmproxy.org/stable/howto-install-system-trusted-ca-android/)

For Android 14, you can use this repo: https://github.com/pwnlogs/cert-fixer

## Hook client with mitmproxy

Set your server address and port in `redirect_server.py`

Install [WireGuard](https://wireguard.com/install/#android-play-store-f-droid) on client, then run mitmproxy:

```
mitmweb -m wireguard --no-http2 -s redirect_server.py --set termlog_verbosity=warn --ignore [Your IP address]
```

Thanks to https://github.com/DennouNeko, discoverer of a way to make the club's chat available

It also works as a packet dumper. You can save the flow file for further works.

## IPv6

If IPv6 is enabled on your network, redirection may not work

The problem can be avoided by disabling IPv6 on the Android side by changing the WireGuard settings

Reference: https://www.reddit.com/r/androiddev/comments/k15y0a/disable_ipv6_over_wifi

Big thanks to u/Swedophone

### Steps:

---

1. Change interface address to fd00::1/128

   Leave the DNS address field blank

3. Create a dummy peer with Allowed IPs set to 2000::/3 between the mitmproxy peer

   Any public key can be used

4. Add mitmproxy peers at the very end

---

Example:

![converted](https://github.com/user-attachments/assets/0098c150-b148-4986-997a-5d3d8ea7d326)
