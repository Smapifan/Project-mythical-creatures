import os
import json
import subprocess
from collections import Counter

# Ergebnisse in Results/
output_dir = "Results"
os.makedirs(output_dir, exist_ok=True)

# Aktuellen Branch herausfinden
branch = subprocess.check_output(
    ["git", "rev-parse", "--abbrev-ref", "HEAD"]
).decode().strip()

# Blacklist für "other"
blacklist_exts = ['.zip', '.exe', '.dll']

filetypes = []
for root, dirs, files in os.walk('.'):
    if '.git' in root or '.github' in root:
        continue
    for f in files:
        ext = f[f.rfind('.'):].lower() if '.' in f else '(no ext)'
        if ext in blacklist_exts:
            ext = 'other'
        filetypes.append(ext)

if not filetypes:
    filetypes.append('(no files found)')

counter = Counter(filetypes)
total_files = sum(counter.values())

result = {
    "branch": branch,
    "filetypes": dict(counter),
    "total_files": total_files
}

json_path = os.path.join(output_dir, f"Result{branch}.json")
with open(json_path, "w", encoding="utf-8") as jf:
    json.dump(result, jf, indent=2)

print(f"Branch '{branch}' gescannt. JSON gespeichert unter {json_path}")
