import os
import json
import subprocess
from pathlib import Path
import tempfile
import shutil

# Dateien zählen
def count_lines(file_path):
    try:
        with open(file_path, "r", encoding="utf-8", errors="ignore") as f:
            return sum(1 for _ in f)
    except Exception:
        return 0

blacklist_exts = {".png", ".jpg", ".jpeg", ".gif", ".bmp", ".exe", ".dll"}

# Aktuellen Branchnamen holen
branch = os.getenv("GITHUB_REF_NAME", "unknown")

# Ordner All_Code auf main erstellen
outdir = Path("All_Code")
outdir.mkdir(exist_ok=True)

# Ziel-Dateiname
json_path = outdir / f"Lines_{branch}.json"

# --- Mit Worktree Branch auschecken ---
tmpdir = tempfile.mkdtemp(prefix="worktree_")
try:
    subprocess.run(["git", "fetch", "origin", branch], check=True)
    subprocess.run(["git", "worktree", "add", tmpdir, f"origin/{branch}"], check=True)

    total_lines = 0
    ext_counter = {}

    for root, dirs, files in os.walk(tmpdir):
        if ".git" in root:
            continue
        for f in files:
            ext = os.path.splitext(f)[1].lower() if "." in f else "(no ext)"
            if ext in blacklist_exts:
                ext = "other"
            path = os.path.join(root, f)  # ✅ eigene Zeile
            lines = count_lines(path)
            total_lines += lines
            ext_counter[ext] = ext_counter.get(ext, 0) + lines

    result = {
        "branch": branch,
        "total_lines": total_lines,
        "lines_by_ext": ext_counter,
    }

    with open(json_path, "w", encoding="utf-8") as jf:
        json.dump(result, jf, indent=2, ensure_ascii=False)

    print(f"Gespeichert: {json_path}")

finally:
    subprocess.run(["git", "worktree", "remove", tmpdir, "--force"], check=True)
    shutil.rmtree(tmpdir, ignore_errors=True)            for f in files:
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

        json_path = os.path.join(output_dir, f"Lines_{branch}.json")
        with open(json_path, "w", encoding="utf-8") as jf:
            json.dump(result, jf, indent=2)

        print(f"Branch '{branch}' gescannt, JSON gespeichert unter {json_path}")

        # Worktree wieder entfernen
        subprocess.run(["git", "worktree", "remove", tmpdir, "--force"], check=True)            path = os.path.join(root, f)
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
