import os
import json
import subprocess
from pathlib import Path

output_dir = "All_Code"
blacklist_exts = ['.zip', '.exe', '.dll']

# Hole alle Branches
branches = subprocess.check_output(
    ["git", "branch", "-r"]
).decode().splitlines()
branches = [b.strip().replace("origin/", "") for b in branches if "origin/" in b and "HEAD" not in b]

def count_lines(path):
    try:
        return sum(1 for _ in open(path, encoding="utf-8", errors="ignore"))
    except:
        return 0

# Stelle sicher, dass der Ordner immer existiert
os.makedirs(output_dir, exist_ok=True)

for branch in branches:
    subprocess.run(["git", "checkout", branch], check=True)

    total_lines = 0
    ext_counter = {}

    for root, dirs, files in os.walk("."):
        if ".git" in root:
            continue
        for f in files:
            ext = os.path.splitext(f)[1].lower() if "." in f else "(no ext)"
            if ext in blacklist_exts:
                ext = "other"
            path = os.path.join(root, f)
            lines = count_lines(path)
            total_lines += lines
            ext_counter[ext] = ext_counter.get(ext, 0) + lines

    result = {
        "branch": branch,
        "total_lines": total_lines,
        "lines_by_ext": ext_counter
    }

    # Speichern IMMER ins lokale Verzeichnis
    json_path = os.path.join(output_dir, f"Lines_{branch}.json")
    os.makedirs(os.path.dirname(json_path), exist_ok=True)
    with open(json_path, "w", encoding="utf-8") as jf:
        json.dump(result, jf, indent=2)

    print(f"Branch '{branch}' gescannt, JSON gespeichert unter {json_path}")

# Am Ende zurück auf main
subprocess.run(["git", "checkout", "main"], check=True)
