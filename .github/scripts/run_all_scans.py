import os
import json
from datetime import datetime

# === Setup ===
BASE_DIR = os.getenv("BASE_DIR", os.getcwd())
OUTPUT_DIR = os.getenv("OUTPUT_DIR", os.path.join(BASE_DIR, "other"))
os.makedirs(OUTPUT_DIR, exist_ok=True)

print("🚀 Script gestartet")
print("BASE_DIR:", BASE_DIR)
print("OUTPUT_DIR:", OUTPUT_DIR)

# === 1️⃣ FILETYPE SCAN ===
def run_filetype_scan():
    print("📂 Running filetype scan...")

    file_data = {}
    total_files = 0

    for root, _, files in os.walk(BASE_DIR):
        if ".git" in root or ".github" in root:
            continue
        for file in files:
            ext = os.path.splitext(file)[1].lower() or "no_extension"
            file_data.setdefault(ext, 0)
            file_data[ext] += 1
            total_files += 1

    output_path = os.path.join(OUTPUT_DIR, "filetype.json")
    with open(output_path, "w", encoding="utf-8") as f:
        json.dump({"total_files": total_files, "filetypes": file_data}, f, indent=2)

    print(f"✅ Filetype scan complete — saved to {output_path}")
    return output_path, file_data, total_files

# === 2️⃣ LINECOUNT SCAN ===
def run_linecount_scan():
    print("📄 Running linecount scan...")

    line_data = {}
    total_lines = 0

    for root, _, files in os.walk(BASE_DIR):
        if ".git" in root or ".github" in root:
            continue
        for file in files:
            if file.endswith((".py", ".json", ".yml", ".yaml", ".js", ".ts", ".cs", ".cpp", ".java", ".html", ".css")):
                path = os.path.join(root, file)
                try:
                    with open(path, "r", encoding="utf-8") as f:
                        lines = f.readlines()
                    count = len(lines)
                    line_data[path.replace(BASE_DIR + os.sep, "")] = count
                    total_lines += count
                except Exception as e:
                    print(f"⚠️ Could not read {path}: {e}")

    output_path = os.path.join(OUTPUT_DIR, "linecount.json")
    with open(output_path, "w", encoding="utf-8") as f:
        json.dump({"total_lines": total_lines, "files": line_data}, f, indent=2)

    print(f"✅ Linecount scan complete — saved to {output_path}")
    return output_path, line_data, total_lines

# === 3️⃣ MARKDOWN GENERATOR: FILETYPE ===
def generate_filetype_markdown(file_data, total_files):
    print("📝 Generating filetype markdown...")

    md = [
        "# 📦 Filetype Report",
        "",
        f"**Total Files:** {total_files}",
        "",
        "| File Extension | Count |",
        "|----------------|--------|",
    ]

    for ext, count in sorted(file_data.items(), key=lambda x: x[0]):
        md.append(f"| `{ext}` | {count} |")

    md.append("")
    md.append(f"_Generated on {datetime.utcnow().strftime('%Y-%m-%d %H:%M:%S UTC')}_")

    output_path = os.path.join(OUTPUT_DIR, "ALL_FILE_REPORT.md")
    with open(output_path, "w", encoding="utf-8") as f:
        f.write("\n".join(md))

    print(f"✅ Markdown report created — saved to {output_path}")
    return output_path

# === 4️⃣ MARKDOWN GENERATOR: LINECOUNT ===
def generate_linecount_markdown(line_data, total_lines):
    print("🧾 Generating linecount markdown...")

    md = [
        "# 📊 Code Linecount Report",
        "",
        f"**Total Lines:** {total_lines}",
        "",
        "| File | Lines |",
        "|------|--------|",
    ]

    for path, count in sorted(line_data.items(), key=lambda x: x[0]):
        md.append(f"| `{path}` | {count} |")

    md.append("")
    md.append(f"_Generated on {datetime.utcnow().strftime('%Y-%m-%d %H:%M:%S UTC')}_")

    output_path = os.path.join(OUTPUT_DIR, "FILECODE.md")
    with open(output_path, "w", encoding="utf-8") as f:
        f.write("\n".join(md))

    print(f"✅ Linecount markdown created — saved to {output_path}")
    return output_path

# === MAIN EXECUTION FLOW ===
if __name__ == "__main__":
    print("🚀 Starting all scans...")

    # Run Linecount first
    line_json, line_data, total_lines = run_linecount_scan()
    line_md = generate_linecount_markdown(line_data, total_lines)

    # Then run Filetype scan
    file_json, file_data, total_files = run_filetype_scan()
    file_md = generate_filetype_markdown(file_data, total_files)

    print("\n🎉 All scans and markdown generations finished successfully!")
